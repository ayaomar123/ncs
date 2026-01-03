import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ApiClient } from '../../../core/services/api-client.service';

@Component({
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
  <div class="container py-10">
    <h1 class="text-2xl font-semibold">Contact</h1>
    <p class="mt-2 text-slate-700">Send us a message and we’ll get back to you.</p>

    <form class="mt-8 grid gap-4 max-w-xl" [formGroup]="form" (ngSubmit)="submit()">
      <label class="block">
        <span class="text-sm font-medium">Name</span>
        <input class="mt-1 w-full rounded border border-slate-300 px-3 py-2" type="text" formControlName="name" />
        <span class="mt-1 block text-xs text-red-600" *ngIf="form.controls.name.touched && form.controls.name.invalid">Name is required.</span>
      </label>

      <label class="block">
        <span class="text-sm font-medium">Email</span>
        <input class="mt-1 w-full rounded border border-slate-300 px-3 py-2" type="email" formControlName="email" />
        <span class="mt-1 block text-xs text-red-600" *ngIf="form.controls.email.touched && form.controls.email.invalid">Valid email is required.</span>
      </label>

      <label class="block">
        <span class="text-sm font-medium">Subject</span>
        <input class="mt-1 w-full rounded border border-slate-300 px-3 py-2" type="text" formControlName="subject" />
        <span class="mt-1 block text-xs text-red-600" *ngIf="form.controls.subject.touched && form.controls.subject.invalid">Subject is required.</span>
      </label>

      <label class="block">
        <span class="text-sm font-medium">Message</span>
        <textarea class="mt-1 w-full rounded border border-slate-300 px-3 py-2" rows="6" formControlName="message"></textarea>
        <span class="mt-1 block text-xs text-red-600" *ngIf="form.controls.message.touched && form.controls.message.invalid">Message is required.</span>
      </label>

      <button class="rounded bg-slate-900 px-4 py-2 text-white hover:bg-slate-800 focus:outline-none focus:ring-2 focus:ring-slate-400" type="submit" [disabled]="form.invalid || sending">
        {{ sending ? 'Sending…' : 'Send message' }}
      </button>

      <div class="text-sm text-emerald-700" *ngIf="sent">Thank you — we’ve received your message.</div>
    </form>
  </div>
  `,
})
export class ContactComponent {
  sending = false;
  sent = false;

  form = new FormGroup({
    name: new FormControl('', { nonNullable: true, validators: [Validators.required] }),
    email: new FormControl('', { nonNullable: true, validators: [Validators.required, Validators.email] }),
    subject: new FormControl('', { nonNullable: true, validators: [Validators.required] }),
    message: new FormControl('', { nonNullable: true, validators: [Validators.required] }),
  });

  constructor(private api: ApiClient) {}

  submit() {
    this.form.markAllAsTouched();
    if (this.form.invalid) {
      return;
    }

    this.sending = true;
    this.sent = false;

    this.api.post<{ id: string }>('/api/contact', this.form.getRawValue()).subscribe({
      next: () => {
        this.sent = true;
        this.form.reset();
      },
      complete: () => {
        this.sending = false;
      }
    });
  }
}
