import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ApiClient } from '../../../core/services/api-client.service';
import { AppealsService } from '../../../core/services/appeals.service';
import { Appeal } from '../../../core/models/appeal';

@Component({
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
  <div class="container py-10">
    <h1 class="text-2xl font-semibold">Donate</h1>
    <p class="mt-2 text-slate-700">Create a donation intent (MVP). No payment is processed yet.</p>

    <form class="mt-8 grid gap-4 max-w-xl" [formGroup]="form" (ngSubmit)="submit()">
      <label class="block">
        <span class="text-sm font-medium">Amount</span>
        <input class="mt-1 w-full rounded border border-slate-300 px-3 py-2" type="number" min="1" formControlName="amount" />
        <span class="mt-1 block text-xs text-red-600" *ngIf="form.controls.amount.touched && form.controls.amount.invalid">Amount must be greater than 0.</span>
      </label>

      <label class="block">
        <span class="text-sm font-medium">Currency</span>
        <input class="mt-1 w-full rounded border border-slate-300 px-3 py-2" type="text" formControlName="currency" />
      </label>

      <label class="block">
        <span class="text-sm font-medium">Type</span>
        <select class="mt-1 w-full rounded border border-slate-300 px-3 py-2" formControlName="type">
          <option [ngValue]="0">One-off</option>
          <option [ngValue]="1">Monthly</option>
        </select>
      </label>

      <label class="block">
        <span class="text-sm font-medium">Category</span>
        <select class="mt-1 w-full rounded border border-slate-300 px-3 py-2" formControlName="category">
          <option [ngValue]="0">Food</option>
          <option [ngValue]="1">Water</option>
          <option [ngValue]="2">Medical</option>
          <option [ngValue]="3">Shelter</option>
          <option [ngValue]="4">Orphan</option>
        </select>
      </label>

      <label class="block">
        <span class="text-sm font-medium">Appeal (optional)</span>
        <select class="mt-1 w-full rounded border border-slate-300 px-3 py-2" formControlName="appealId">
          <option value="">No specific appeal</option>
          <option *ngFor="let a of appeals" [value]="a.id">{{ a.title }}</option>
        </select>
      </label>

      <label class="block">
        <span class="text-sm font-medium">Your name</span>
        <input class="mt-1 w-full rounded border border-slate-300 px-3 py-2" type="text" formControlName="donorName" />
      </label>

      <label class="block">
        <span class="text-sm font-medium">Your email</span>
        <input class="mt-1 w-full rounded border border-slate-300 px-3 py-2" type="email" formControlName="donorEmail" />
        <span class="mt-1 block text-xs text-red-600" *ngIf="form.controls.donorEmail.touched && form.controls.donorEmail.invalid">Valid email is required.</span>
      </label>

      <button class="rounded bg-emerald-600 px-4 py-2 text-white hover:bg-emerald-700 focus:outline-none focus:ring-2 focus:ring-emerald-400" type="submit" [disabled]="form.invalid || sending">
        {{ sending ? 'Submitting…' : 'Submit donation intent' }}
      </button>
    </form>
  </div>
  `,
})
export class DonateComponent implements OnInit {
  sending = false;
  appeals: Appeal[] = [];

  form = new FormGroup({
    amount: new FormControl<number>(50, { nonNullable: true, validators: [Validators.required, Validators.min(1)] }),
    currency: new FormControl<string>('GBP', { nonNullable: true, validators: [Validators.required] }),
    type: new FormControl<number>(0, { nonNullable: true }),
    category: new FormControl<number>(0, { nonNullable: true }),
    appealId: new FormControl<string>(''),
    donorName: new FormControl<string>('', { nonNullable: true, validators: [Validators.required] }),
    donorEmail: new FormControl<string>('', { nonNullable: true, validators: [Validators.required, Validators.email] }),
  });

  constructor(private api: ApiClient, private appealsService: AppealsService, private router: Router) {}

  ngOnInit(): void {
    this.appealsService.list({ pageNumber: 1, pageSize: 50 }).subscribe(r => (this.appeals = r.items));
  }

  submit() {
    this.form.markAllAsTouched();
    if (this.form.invalid) {
      return;
    }

    this.sending = true;
    const raw = this.form.getRawValue();
    const payload = {
      amount: raw.amount,
      currency: raw.currency,
      type: raw.type,
      category: raw.category,
      appealId: raw.appealId ? raw.appealId : null,
      donorName: raw.donorName,
      donorEmail: raw.donorEmail,
    };

    this.api.post<{ donationRequestId: string; redirectUrl: string }>('/api/donations', payload).subscribe({
      next: (r) => {
        this.router.navigateByUrl(`${r.redirectUrl}?donationRequestId=${r.donationRequestId}`);
      },
      complete: () => {
        this.sending = false;
      }
    });
  }
}
