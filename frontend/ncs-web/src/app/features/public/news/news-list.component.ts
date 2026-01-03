import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { PostsService } from '../../../core/services/posts.service';
import { BlogPost } from '../../../core/models/blog-post';

@Component({
  standalone: true,
  imports: [CommonModule, RouterLink],
  template: `
  <div class="container py-10">
    <h1 class="text-2xl font-semibold">News & Blog</h1>
    <p class="mt-2 text-slate-700">Updates from NCS and stories from the field.</p>

    <div class="mt-8 grid gap-6 md:grid-cols-2 lg:grid-cols-3">
      <a *ngFor="let p of posts" [routerLink]="['/news', p.slug]" class="rounded-lg border border-slate-200 bg-white p-5 shadow-sm hover:bg-slate-50">
        <div class="font-semibold">{{ p.title }}</div>
        <div class="mt-2 text-sm text-slate-600">{{ p.excerpt }}</div>
        <div class="mt-4 flex flex-wrap gap-2">
          <span *ngFor="let t of p.tags" class="rounded bg-slate-100 px-2 py-1 text-xs text-slate-700">{{ t }}</span>
        </div>
      </a>
    </div>
  </div>
  `,
})
export class NewsListComponent implements OnInit {
  posts: BlogPost[] = [];

  constructor(private postsService: PostsService) {}

  ngOnInit(): void {
    this.postsService.list({ pageNumber: 1, pageSize: 24 }).subscribe(r => (this.posts = r.items));
  }
}
