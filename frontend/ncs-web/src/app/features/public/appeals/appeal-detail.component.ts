import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { AppealsService } from '../../../core/services/appeals.service';
import { Appeal } from '../../../core/models/appeal';

@Component({
  standalone: true,
  imports: [CommonModule, RouterLink],
  template: `
  <div class="container py-10" *ngIf="appeal; else loading">
    <a routerLink="/appeals" class="text-sm text-emerald-700 hover:text-emerald-800">← Back to appeals</a>

    <div class="mt-4 flex flex-wrap items-center gap-3">
      <h1 class="text-2xl font-semibold">{{ appeal.title }}</h1>
      <span *ngIf="appeal.isUrgent" class="rounded bg-red-100 px-2 py-1 text-xs font-semibold text-red-700">Urgent</span>
    </div>

    <p class="mt-2 text-slate-700">{{ appeal.summary }}</p>

    <div class="mt-6 grid gap-8 lg:grid-cols-3">
      <div class="lg:col-span-2">
        <div class="prose max-w-none">
          <p>{{ appeal.description }}</p>
        </div>
      </div>

      <aside class="rounded-lg border border-slate-200 bg-white p-5 shadow-sm">
        <div class="text-sm text-slate-600">Target</div>
        <div class="text-xl font-semibold">{{ appeal.targetAmount | number:'1.0-0' }}</div>

        <div class="mt-3 text-sm text-slate-600">Raised</div>
        <div class="text-xl font-semibold">{{ appeal.raisedAmount | number:'1.0-0' }}</div>

        <a routerLink="/donate" class="mt-6 inline-block w-full rounded bg-emerald-600 px-4 py-2 text-center text-white hover:bg-emerald-700 focus:outline-none focus:ring-2 focus:ring-emerald-400">
          Donate to this appeal
        </a>
      </aside>
    </div>
  </div>

  <ng-template #loading>
    <div class="container py-10 text-sm text-slate-600">Loading…</div>
  </ng-template>
  `,
})
export class AppealDetailComponent implements OnInit {
  appeal?: Appeal;

  constructor(private route: ActivatedRoute, private appeals: AppealsService) {}

  ngOnInit(): void {
    const slug = this.route.snapshot.paramMap.get('slug') ?? '';
    this.appeals.getBySlug(slug).subscribe(a => (this.appeal = a));
  }
}
