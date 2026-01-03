import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { AppealsService } from '../../../core/services/appeals.service';
import { Appeal } from '../../../core/models/appeal';

@Component({
  standalone: true,
  imports: [CommonModule, RouterLink, ReactiveFormsModule],
  template: `
  <div class="container py-10">
    <h1 class="text-2xl font-semibold">Emergency Appeals</h1>
    <p class="mt-2 text-slate-700">Browse current appeals and respond to urgent needs.</p>

    <form class="mt-6 grid gap-4 md:grid-cols-3" [formGroup]="filters" (ngSubmit)="applyFilters()">
      <label class="block">
        <span class="text-sm font-medium">Country</span>
        <input class="mt-1 w-full rounded border border-slate-300 px-3 py-2" type="text" formControlName="countryTag" />
      </label>

      <label class="block">
        <span class="text-sm font-medium">Urgent only</span>
        <select class="mt-1 w-full rounded border border-slate-300 px-3 py-2" formControlName="isUrgent">
          <option value="">All</option>
          <option value="true">Urgent</option>
          <option value="false">Not urgent</option>
        </select>
      </label>

      <div class="flex items-end">
        <button class="w-full rounded bg-slate-900 px-4 py-2 text-white hover:bg-slate-800 focus:outline-none focus:ring-2 focus:ring-slate-400" type="submit">Filter</button>
      </div>
    </form>

    <div class="mt-8 grid gap-6 md:grid-cols-2 lg:grid-cols-3">
      <a *ngFor="let a of appeals" [routerLink]="['/appeals', a.slug]" class="rounded-lg border border-slate-200 bg-white p-5 shadow-sm hover:bg-slate-50">
        <div class="flex items-start justify-between gap-3">
          <div class="font-semibold">{{ a.title }}</div>
          <span *ngIf="a.isUrgent" class="rounded bg-red-100 px-2 py-1 text-xs font-semibold text-red-700">Urgent</span>
        </div>
        <div class="mt-2 text-sm text-slate-600">{{ a.summary }}</div>
        <div class="mt-4 text-xs text-slate-500">{{ a.countryTag || '—' }}</div>
      </a>
    </div>
  </div>
  `,
})
export class AppealsListComponent implements OnInit {
  appeals: Appeal[] = [];

  filters = new FormGroup({
    countryTag: new FormControl<string>(''),
    isUrgent: new FormControl<string>(''),
  });

  constructor(private appealsService: AppealsService) {}

  ngOnInit(): void {
    this.load();
  }

  applyFilters() {
    this.load();
  }

  private load() {
    const countryTag = this.filters.value.countryTag?.trim() || undefined;
    const isUrgentValue = this.filters.value.isUrgent;
    const isUrgent = isUrgentValue === '' ? undefined : isUrgentValue === 'true';

    this.appealsService.list({ pageNumber: 1, pageSize: 24, countryTag, isUrgent }).subscribe(r => {
      this.appeals = r.items;
    });
  }
}
