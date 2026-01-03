import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AppealsService } from '../../../core/services/appeals.service';
import { Appeal } from '../../../core/models/appeal';

@Component({
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
  <div class="container py-10">
    <h1 class="text-2xl font-semibold">Manage appeals</h1>

    <div class="mt-8 grid gap-8 lg:grid-cols-2">
      <div>
        <h2 class="text-lg font-semibold">Existing</h2>

        <div class="mt-4 space-y-3">
          <button *ngFor="let a of appeals" type="button" (click)="select(a)" class="w-full rounded border border-slate-200 bg-white p-4 text-left hover:bg-slate-50">
            <div class="flex items-center justify-between gap-3">
              <div class="font-medium">{{ a.title }}</div>
              <span *ngIf="a.isUrgent" class="rounded bg-red-100 px-2 py-1 text-xs font-semibold text-red-700">Urgent</span>
            </div>
            <div class="mt-1 text-sm text-slate-600">{{ a.summary }}</div>
          </button>
        </div>
      </div>

      <div>
        <h2 class="text-lg font-semibold">{{ selectedId ? 'Edit' : 'Create' }}</h2>

        <form class="mt-4 grid gap-4" [formGroup]="form" (ngSubmit)="save()">
          <label class="block">
            <span class="text-sm font-medium">Title</span>
            <input class="mt-1 w-full rounded border border-slate-300 px-3 py-2" formControlName="title" />
          </label>

          <label class="block">
            <span class="text-sm font-medium">Summary</span>
            <input class="mt-1 w-full rounded border border-slate-300 px-3 py-2" formControlName="summary" />
          </label>

          <label class="block">
            <span class="text-sm font-medium">Description</span>
            <textarea class="mt-1 w-full rounded border border-slate-300 px-3 py-2" rows="6" formControlName="description"></textarea>
          </label>

          <label class="block">
            <span class="text-sm font-medium">Country tag</span>
            <input class="mt-1 w-full rounded border border-slate-300 px-3 py-2" formControlName="countryTag" />
          </label>

          <label class="flex items-center gap-2">
            <input type="checkbox" class="h-4 w-4" formControlName="isUrgent" />
            <span class="text-sm">Urgent</span>
          </label>

          <div class="grid gap-4 md:grid-cols-2">
            <label class="block">
              <span class="text-sm font-medium">Target amount</span>
              <input class="mt-1 w-full rounded border border-slate-300 px-3 py-2" type="number" min="0" formControlName="targetAmount" />
            </label>
            <label class="block">
              <span class="text-sm font-medium">Raised amount</span>
              <input class="mt-1 w-full rounded border border-slate-300 px-3 py-2" type="number" min="0" formControlName="raisedAmount" />
            </label>
          </div>

          <label class="block">
            <span class="text-sm font-medium">Cover image URL</span>
            <input class="mt-1 w-full rounded border border-slate-300 px-3 py-2" formControlName="coverImageUrl" />
          </label>

          <label class="block">
            <span class="text-sm font-medium">Status</span>
            <select class="mt-1 w-full rounded border border-slate-300 px-3 py-2" formControlName="status">
              <option [ngValue]="0">Draft</option>
              <option [ngValue]="1">Published</option>
              <option [ngValue]="2">Archived</option>
            </select>
          </label>

          <div class="flex flex-wrap gap-3">
            <button class="rounded bg-slate-900 px-4 py-2 text-white hover:bg-slate-800" type="submit" [disabled]="form.invalid || saving">
              {{ saving ? 'Saving…' : 'Save' }}
            </button>

            <button class="rounded border border-slate-300 px-4 py-2" type="button" (click)="reset()">New</button>

            <button *ngIf="selectedId" class="rounded border border-red-300 px-4 py-2 text-red-700" type="button" (click)="remove()">Delete</button>
          </div>
        </form>

        <div class="mt-3 text-sm text-emerald-700" *ngIf="message">{{ message }}</div>
      </div>
    </div>
  </div>
  `,
})
export class AdminAppealsComponent implements OnInit {
  appeals: Appeal[] = [];
  selectedId: string | null = null;
  saving = false;
  message = '';

  form = new FormGroup({
    title: new FormControl('', { nonNullable: true, validators: [Validators.required] }),
    summary: new FormControl('', { nonNullable: true, validators: [Validators.required] }),
    description: new FormControl('', { nonNullable: true, validators: [Validators.required] }),
    countryTag: new FormControl(''),
    isUrgent: new FormControl(false, { nonNullable: true }),
    targetAmount: new FormControl(0, { nonNullable: true }),
    raisedAmount: new FormControl(0, { nonNullable: true }),
    coverImageUrl: new FormControl(''),
    status: new FormControl<number>(0, { nonNullable: true }),
  });

  constructor(private appealsService: AppealsService) {}

  ngOnInit(): void {
    this.reload();
  }

  select(a: Appeal) {
    this.selectedId = a.id;
    this.form.patchValue({
      title: a.title,
      summary: a.summary,
      description: a.description,
      countryTag: a.countryTag ?? '',
      isUrgent: a.isUrgent,
      targetAmount: a.targetAmount,
      raisedAmount: a.raisedAmount,
      coverImageUrl: a.coverImageUrl ?? '',
      status: a.status,
    });
  }

  reset() {
    this.selectedId = null;
    this.form.reset({
      title: '',
      summary: '',
      description: '',
      countryTag: '',
      isUrgent: false,
      targetAmount: 0,
      raisedAmount: 0,
      coverImageUrl: '',
      status: 0,
    });
    this.message = '';
  }

  save() {
    this.form.markAllAsTouched();
    if (this.form.invalid) return;

    this.saving = true;
    this.message = '';

    const raw = this.form.getRawValue();
    const payload: any = {
      id: this.selectedId ?? undefined,
      title: raw.title,
      summary: raw.summary,
      description: raw.description,
      countryTag: raw.countryTag || null,
      isUrgent: raw.isUrgent,
      targetAmount: raw.targetAmount,
      raisedAmount: raw.raisedAmount,
      coverImageUrl: raw.coverImageUrl || null,
      galleryUrls: [],
      status: raw.status,
    };

    const req$ = this.selectedId
      ? this.appealsService.adminUpdate(this.selectedId, payload)
      : this.appealsService.adminCreate(payload);

    req$.subscribe({
      next: () => {
        this.message = 'Saved.';
        this.reload();
      },
      complete: () => {
        this.saving = false;
      }
    });
  }

  remove() {
    if (!this.selectedId) return;

    this.appealsService.adminDelete(this.selectedId).subscribe({
      next: () => {
        this.reset();
        this.reload();
      }
    });
  }

  private reload() {
    this.appealsService.adminList({ pageNumber: 1, pageSize: 50 }).subscribe(r => {
      this.appeals = r.items;
    });
  }
}
