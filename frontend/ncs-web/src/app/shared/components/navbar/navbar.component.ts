import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [RouterLink, RouterLinkActive],
  template: `
  <header class="fixed inset-x-0 top-0 z-50 border-b border-slate-200 bg-white/90 backdrop-blur">
    <div class="container flex h-16 items-center justify-between">
      <a routerLink="/" class="font-semibold text-slate-900">Nationwide Care Solutions</a>

      <nav class="flex items-center gap-4 text-sm">
        <a routerLink="/appeals" routerLinkActive="font-semibold" class="hover:text-slate-900 text-slate-700">Appeals</a>
        <a routerLink="/news" routerLinkActive="font-semibold" class="hover:text-slate-900 text-slate-700">News</a>
        <a routerLink="/contact" routerLinkActive="font-semibold" class="hover:text-slate-900 text-slate-700">Contact</a>
        <a routerLink="/donate" class="rounded bg-emerald-600 px-3 py-2 text-white hover:bg-emerald-700 focus:outline-none focus:ring-2 focus:ring-emerald-400">Donate</a>
      </nav>
    </div>
  </header>
  `,
})
export class NavbarComponent {}
