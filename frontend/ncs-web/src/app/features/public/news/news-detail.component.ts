import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { PostsService } from '../../../core/services/posts.service';
import { BlogPost } from '../../../core/models/blog-post';

@Component({
  standalone: true,
  imports: [CommonModule, RouterLink],
  template: `
  <div class="container py-10" *ngIf="post; else loading">
    <a routerLink="/news" class="text-sm text-emerald-700 hover:text-emerald-800">← Back to news</a>

    <h1 class="mt-4 text-2xl font-semibold">{{ post.title }}</h1>
    <p class="mt-2 text-slate-700">{{ post.excerpt }}</p>

    <div class="mt-6 whitespace-pre-wrap rounded-lg border border-slate-200 bg-white p-5 text-slate-800">
      {{ post.content }}
    </div>
  </div>

  <ng-template #loading>
    <div class="container py-10 text-sm text-slate-600">Loading…</div>
  </ng-template>
  `,
})
export class NewsDetailComponent implements OnInit {
  post?: BlogPost;

  constructor(private route: ActivatedRoute, private posts: PostsService) {}

  ngOnInit(): void {
    const slug = this.route.snapshot.paramMap.get('slug') ?? '';
    this.posts.getBySlug(slug).subscribe(p => (this.post = p));
  }
}
