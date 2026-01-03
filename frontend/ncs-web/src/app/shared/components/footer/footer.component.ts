import { Component } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-footer',
  standalone: true,
  imports: [RouterLink],
  template: `
  <footer class="border-t border-slate-200 bg-slate-50">
    <div class="container py-10">
      <div class="flex flex-col gap-6 md:flex-row md:items-center md:justify-between">
        <div>
          <div class="font-semibold">Nationwide Care Solutions (NCS)</div>
          <p class="mt-1 max-w-xl text-sm text-slate-600">A charity supporting vulnerable communities through emergency relief, clean water and long-term care solutions.</p>
        </div>
        <div class="flex flex-wrap gap-4 text-sm">
          <a routerLink="/about" class="text-slate-700 hover:text-slate-900">About</a>
          <a routerLink="/get-involved" class="text-slate-700 hover:text-slate-900">Get involved</a>
          <a routerLink="/admin/login" class="text-slate-700 hover:text-slate-900">Admin</a>
        </div>
      </div>
      <div class="mt-8 text-xs text-slate-500">© {{ year }} Nationwide Care Solutions</div>
    </div>
  </footer>
  `,
})
export class FooterComponent {
  year = new Date().getFullYear();
}
