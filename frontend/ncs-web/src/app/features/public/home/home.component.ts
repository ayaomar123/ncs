import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { AppealsService } from '../../../core/services/appeals.service';
import { Appeal } from '../../../core/models/appeal';

@Component({
  standalone: true,
  imports: [CommonModule, RouterLink],
  template: `
  <section class="bg-slate-50">
    <div class="container py-14">
      <div class="grid gap-10 md:grid-cols-2 md:items-center">
        <div>
          <h1 class="text-3xl font-bold tracking-tight md:text-4xl">Nationwide Care Solutions</h1>
          <p class="mt-4 text-slate-700">
            We support vulnerable communities with emergency relief, clean water projects, and long-term care solutions.
          </p>

          <div class="mt-6 flex flex-wrap gap-3">
            <a routerLink="/donate" class="rounded bg-emerald-600 px-4 py-2 text-white hover:bg-emerald-700 focus:outline-none focus:ring-2 focus:ring-emerald-400">Donate now</a>
            <a routerLink="/sponsor" class="rounded border border-slate-300 px-4 py-2 text-slate-900 hover:bg-white focus:outline-none focus:ring-2 focus:ring-slate-300">Sponsor an orphan</a>
            <a routerLink="/get-involved" class="rounded border border-slate-300 px-4 py-2 text-slate-900 hover:bg-white focus:outline-none focus:ring-2 focus:ring-slate-300">Join our mission</a>
          </div>

          <div class="mt-10 grid grid-cols-3 gap-4">
            <div class="rounded bg-white p-4 shadow-sm">
              <div class="text-2xl font-semibold">120+</div>
              <div class="text-xs text-slate-600">Families supported</div>
            </div>
            <div class="rounded bg-white p-4 shadow-sm">
              <div class="text-2xl font-semibold">8</div>
              <div class="text-xs text-slate-600">Active projects</div>
            </div>
            <div class="rounded bg-white p-4 shadow-sm">
              <div class="text-2xl font-semibold">5</div>
              <div class="text-xs text-slate-600">Countries reached</div>
            </div>
          </div>
        </div>

        <div class="rounded-lg bg-white p-6 shadow-sm">
          <h2 class="text-lg font-semibold">Urgent appeals</h2>
          <p class="mt-1 text-sm text-slate-600">Help us respond quickly where the need is greatest.</p>

          <div class="mt-6 space-y-4">
            <ng-container *ngIf="urgentAppeals.length; else loading">
              <a *ngFor="let a of urgentAppeals" [routerLink]="['/appeals', a.slug]" class="block rounded border border-slate-200 p-4 hover:bg-slate-50">
                <div class="flex items-start justify-between gap-4">
                  <div>
                    <div class="font-medium">{{ a.title }}</div>
                    <div class="mt-1 text-sm text-slate-600">{{ a.summary }}</div>
                  </div>
                  <div class="shrink-0 rounded bg-red-100 px-2 py-1 text-xs font-semibold text-red-700">Urgent</div>
                </div>
              </a>
            </ng-container>
            <ng-template #loading>
              <div class="text-sm text-slate-600">Loading…</div>
            </ng-template>
          </div>
        </div>
      </div>
    </div>
  </section>

  <section>
    <div class="container py-14">
      <div class="flex items-end justify-between gap-6">
        <div>
          <h2 class="text-xl font-semibold">Featured appeals</h2>
          <p class="mt-1 text-sm text-slate-600">Explore current appeals and see how you can help.</p>
        </div>
        <a routerLink="/appeals" class="text-sm font-medium text-emerald-700 hover:text-emerald-800">View all</a>
      </div>

      <div class="mt-8 grid gap-6 md:grid-cols-2 lg:grid-cols-3">
        <a *ngFor="let a of featuredAppeals" [routerLink]="['/appeals', a.slug]" class="rounded-lg border border-slate-200 bg-white p-5 shadow-sm hover:bg-slate-50">
          <div class="font-semibold">{{ a.title }}</div>
          <div class="mt-2 text-sm text-slate-600">{{ a.summary }}</div>
          <div class="mt-4 text-xs text-slate-500">
            Raised {{ a.raisedAmount | number:'1.0-0' }} / {{ a.targetAmount | number:'1.0-0' }}
          </div>
        </a>
      </div>
    </div>
  </section>
  `,
})
export class HomeComponent implements OnInit {
  urgentAppeals: Appeal[] = [];
  featuredAppeals: Appeal[] = [];

  constructor(private appeals: AppealsService) {}

  ngOnInit(): void {
    this.appeals.list({ pageNumber: 1, pageSize: 3, isUrgent: true }).subscribe(r => {
      this.urgentAppeals = r.items;
    });

    this.appeals.list({ pageNumber: 1, pageSize: 6 }).subscribe(r => {
      this.featuredAppeals = r.items;
    });
  }
}
