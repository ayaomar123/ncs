import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';

@Component({
  standalone: true,
  imports: [CommonModule, RouterLink],
  template: `
  <div class="container py-10">
    <h1 class="text-2xl font-semibold">Thank you</h1>
    <p class="mt-2 text-slate-700">Your donation intent has been recorded. Payment integration will be added in Phase 2.</p>

    <div class="mt-6 rounded border border-slate-200 bg-white p-5">
      <div class="text-sm text-slate-600">Donation request ID</div>
      <div class="mt-1 font-mono text-sm">{{ donationRequestId || '—' }}</div>
    </div>

    <a routerLink="/" class="mt-6 inline-block text-emerald-700 hover:text-emerald-800">Back to home</a>
  </div>
  `,
})
export class DonateSuccessComponent {
  donationRequestId = this.route.snapshot.queryParamMap.get('donationRequestId');
  constructor(private route: ActivatedRoute) {}
}
