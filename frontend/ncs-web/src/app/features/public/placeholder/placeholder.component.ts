import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';

@Component({
  standalone: true,
  imports: [CommonModule, RouterLink],
  template: `
  <div class="container py-10">
    <h1 class="text-2xl font-semibold">{{ heading }}</h1>
    <p class="mt-2 text-slate-700">{{ message }}</p>
    <a routerLink="/" class="mt-6 inline-block text-emerald-700 hover:text-emerald-800">Back to home</a>
  </div>
  `,
})
export class PlaceholderComponent {
  heading = this.route.snapshot.data['heading'] ?? 'Coming soon';
  message = this.route.snapshot.data['message'] ?? 'This page is not implemented in MVP yet.';

  constructor(private route: ActivatedRoute) {}
}
