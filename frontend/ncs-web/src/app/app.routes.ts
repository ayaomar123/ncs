import { Routes } from '@angular/router';
import { adminAuthGuard } from './core/guards/admin-auth.guard';

export const routes: Routes = [
  {
    path: '',
    loadComponent: () => import('./features/public/home/home.component').then(m => m.HomeComponent),
    title: 'Nationwide Care Solutions',
  },
  {
    path: 'appeals',
    loadComponent: () => import('./features/public/appeals/appeals-list.component').then(m => m.AppealsListComponent),
    title: 'Emergency Appeals | NCS',
  },
  {
    path: 'appeals/:slug',
    loadComponent: () => import('./features/public/appeals/appeal-detail.component').then(m => m.AppealDetailComponent),
    title: 'Appeal | NCS',
  },
  {
    path: 'news',
    loadComponent: () => import('./features/public/news/news-list.component').then(m => m.NewsListComponent),
    title: 'News & Blog | NCS',
  },
  {
    path: 'news/:slug',
    loadComponent: () => import('./features/public/news/news-detail.component').then(m => m.NewsDetailComponent),
    title: 'News | NCS',
  },
  {
    path: 'contact',
    loadComponent: () => import('./features/public/contact/contact.component').then(m => m.ContactComponent),
    title: 'Contact | NCS',
  },
  {
    path: 'donate',
    loadComponent: () => import('./features/public/donate/donate.component').then(m => m.DonateComponent),
    title: 'Donate | NCS',
  },
  {
    path: 'donate/success',
    loadComponent: () => import('./features/public/donate/donate-success.component').then(m => m.DonateSuccessComponent),
    title: 'Thank you | NCS',
  },

  // Placeholders
  {
    path: 'sponsor',
    loadComponent: () => import('./features/public/placeholder/placeholder.component').then(m => m.PlaceholderComponent),
    data: { heading: 'Sponsor an Orphan', message: 'Coming soon.' },
    title: 'Sponsor | NCS',
  },
  {
    path: 'get-involved',
    loadComponent: () => import('./features/public/placeholder/placeholder.component').then(m => m.PlaceholderComponent),
    data: { heading: 'Get involved', message: 'Coming soon.' },
    title: 'Get involved | NCS',
  },
  {
    path: 'about',
    loadComponent: () => import('./features/public/placeholder/placeholder.component').then(m => m.PlaceholderComponent),
    data: { heading: 'About NCS', message: 'Coming soon.' },
    title: 'About | NCS',
  },

  // Admin
  {
    path: 'admin/login',
    loadComponent: () => import('./features/admin/login/admin-login.component').then(m => m.AdminLoginComponent),
    title: 'Admin login | NCS',
  },
  {
    path: 'admin',
    canActivate: [adminAuthGuard],
    loadComponent: () => import('./features/admin/dashboard/admin-dashboard.component').then(m => m.AdminDashboardComponent),
    title: 'Admin dashboard | NCS',
  },
  {
    path: 'admin/appeals',
    canActivate: [adminAuthGuard],
    loadComponent: () => import('./features/admin/appeals/admin-appeals.component').then(m => m.AdminAppealsComponent),
    title: 'Manage appeals | NCS',
  },
  {
    path: 'admin/posts',
    canActivate: [adminAuthGuard],
    loadComponent: () => import('./features/admin/posts/admin-posts.component').then(m => m.AdminPostsComponent),
    title: 'Manage posts | NCS',
  },
  {
    path: 'admin/media',
    canActivate: [adminAuthGuard],
    loadComponent: () => import('./features/admin/media/admin-media.component').then(m => m.AdminMediaComponent),
    title: 'Media | NCS',
  },

  { path: '**', redirectTo: '' },
];
