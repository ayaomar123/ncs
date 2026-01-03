import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
  <div class="container py-10">
    <h1 class="text-2xl font-semibold">Admin login</h1>

    <form class="mt-8 grid gap-4 max-w-md" [formGroup]="form" (ngSubmit)="submit()">
      <label class="block">
        <span class="text-sm font-medium">Email</span>
        <input class="mt-1 w-full rounded border border-slate-300 px-3 py-2" type="email" formControlName="email" autocomplete="username" />
      </label>

      <label class="block">
        <span class="text-sm font-medium">Password</span>
        <input class="mt-1 w-full rounded border border-slate-300 px-3 py-2" type="password" formControlName="password" autocomplete="current-password" />
      </label>

      <button class="rounded bg-slate-900 px-4 py-2 text-white hover:bg-slate-800 focus:outline-none focus:ring-2 focus:ring-slate-400" type="submit" [disabled]="form.invalid || loading">
        {{ loading ? 'Signing in…' : 'Sign in' }}
      </button>

      <div class="text-sm text-red-600" *ngIf="error">{{ error }}</div>
    </form>
  </div>
  `,
})
export class AdminLoginComponent {
  loading = false;
  error = '';

  form = new FormGroup({
    email: new FormControl('', { nonNullable: true, validators: [Validators.required, Validators.email] }),
    password: new FormControl('', { nonNullable: true, validators: [Validators.required] }),
  });

  constructor(private auth: AuthService, private router: Router) {}

  submit() {
    this.form.markAllAsTouched();
    if (this.form.invalid) {
      return;
    }

    this.loading = true;
    this.error = '';

    const { email, password } = this.form.getRawValue();
    this.auth.login(email, password).subscribe({
      next: () => this.router.navigateByUrl('/admin'),
      error: () => {
        this.error = 'Login failed. Check credentials.';
        this.loading = false;
      },
      complete: () => {
        this.loading = false;
      }
    });
  }
}
