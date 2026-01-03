import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  standalone: true,
  imports: [CommonModule, RouterLink],
  template: `
  <div class="container py-10">
    <div class="flex items-center justify-between">
      <h1 class="text-2xl font-semibold">Admin dashboard</h1>
      <button (click)="logout()" class="rounded border border-slate-300 px-4 py-2 text-slate-900 hover:bg-slate-50">Logout</button>
    </div>

    <div class="mt-8 grid gap-4 md:grid-cols-3">
      <a routerLink="/admin/appeals" class="rounded border border-slate-200 bg-white p-5 shadow-sm hover:bg-slate-50">
        <div class="font-semibold">Appeals</div>
        <div class="mt-1 text-sm text-slate-600">Create and manage appeals.</div>
      </a>
      <a routerLink="/admin/posts" class="rounded border border-slate-200 bg-white p-5 shadow-sm hover:bg-slate-50">
        <div class="font-semibold">Blog posts</div>
        <div class="mt-1 text-sm text-slate-600">Create and manage blog posts.</div>
      </a>
      <a routerLink="/admin/media" class="rounded border border-slate-200 bg-white p-5 shadow-sm hover:bg-slate-50">
        <div class="font-semibold">Media</div>
        <div class="mt-1 text-sm text-slate-600">Upload images (local storage).</div>
      </a>
    </div>
  </div>
  `,
})
export class AdminDashboardComponent {
  constructor(private auth: AuthService, private router: Router) {}

  logout() {
    this.auth.logout();
    this.router.navigateByUrl('/');
  }
}
