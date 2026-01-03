import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { PostsService } from '../../../core/services/posts.service';
import { BlogPost } from '../../../core/models/blog-post';

@Component({
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
  <div class="container py-10">
    <h1 class="text-2xl font-semibold">Manage blog posts</h1>

    <div class="mt-8 grid gap-8 lg:grid-cols-2">
      <div>
        <h2 class="text-lg font-semibold">Existing</h2>

        <div class="mt-4 space-y-3">
          <button *ngFor="let p of posts" type="button" (click)="select(p)" class="w-full rounded border border-slate-200 bg-white p-4 text-left hover:bg-slate-50">
            <div class="flex items-center justify-between gap-3">
              <div class="font-medium">{{ p.title }}</div>
              <span class="rounded px-2 py-1 text-xs" [class]="p.isPublished ? 'bg-emerald-100 text-emerald-800' : 'bg-slate-100 text-slate-700'">
                {{ p.isPublished ? 'Published' : 'Draft' }}
              </span>
            </div>
            <div class="mt-1 text-sm text-slate-600">{{ p.excerpt }}</div>
          </button>
        </div>
      </div>

      <div>
        <h2 class="text-lg font-semibold">{{ selectedId ? 'Edit' : 'Create' }}</h2>

        <form class="mt-4 grid gap-4" [formGroup]="form" (ngSubmit)="save()">
          <label class="block">
            <span class="text-sm font-medium">Title</span>
            <input class="mt-1 w-full rounded border border-slate-300 px-3 py-2" formControlName="title" />
          </label>

          <label class="block">
            <span class="text-sm font-medium">Excerpt</span>
            <input class="mt-1 w-full rounded border border-slate-300 px-3 py-2" formControlName="excerpt" />
          </label>

          <label class="block">
            <span class="text-sm font-medium">Content (markdown or HTML)</span>
            <textarea class="mt-1 w-full rounded border border-slate-300 px-3 py-2" rows="10" formControlName="content"></textarea>
          </label>

          <label class="block">
            <span class="text-sm font-medium">Tags (comma separated)</span>
            <input class="mt-1 w-full rounded border border-slate-300 px-3 py-2" formControlName="tags" />
          </label>

          <label class="flex items-center gap-2">
            <input type="checkbox" class="h-4 w-4" formControlName="isPublished" />
            <span class="text-sm">Published</span>
          </label>

          <div class="flex flex-wrap gap-3">
            <button class="rounded bg-slate-900 px-4 py-2 text-white hover:bg-slate-800" type="submit" [disabled]="form.invalid || saving">
              {{ saving ? 'Saving…' : 'Save' }}
            </button>

            <button class="rounded border border-slate-300 px-4 py-2" type="button" (click)="reset()">New</button>

            <button *ngIf="selectedId" class="rounded border border-red-300 px-4 py-2 text-red-700" type="button" (click)="remove()">Delete</button>
          </div>
        </form>

        <div class="mt-3 text-sm text-emerald-700" *ngIf="message">{{ message }}</div>
      </div>
    </div>
  </div>
  `,
})
export class AdminPostsComponent implements OnInit {
  posts: BlogPost[] = [];
  selectedId: string | null = null;
  saving = false;
  message = '';

  form = new FormGroup({
    title: new FormControl('', { nonNullable: true, validators: [Validators.required] }),
    excerpt: new FormControl('', { nonNullable: true, validators: [Validators.required] }),
    content: new FormControl('', { nonNullable: true, validators: [Validators.required] }),
    tags: new FormControl('', { nonNullable: true }),
    isPublished: new FormControl(false, { nonNullable: true }),
  });

  constructor(private postsService: PostsService) {}

  ngOnInit(): void {
    this.reload();
  }

  select(p: BlogPost) {
    this.selectedId = p.id;
    this.form.patchValue({
      title: p.title,
      excerpt: p.excerpt,
      content: p.content,
      tags: p.tags.join(', '),
      isPublished: p.isPublished,
    });
  }

  reset() {
    this.selectedId = null;
    this.form.reset({ title: '', excerpt: '', content: '', tags: '', isPublished: false });
    this.message = '';
  }

  save() {
    this.form.markAllAsTouched();
    if (this.form.invalid) return;

    this.saving = true;
    this.message = '';

    const raw = this.form.getRawValue();
    const payload: any = {
      id: this.selectedId ?? undefined,
      title: raw.title,
      excerpt: raw.excerpt,
      content: raw.content,
      coverImageUrl: null,
      tags: raw.tags.split(',').map(t => t.trim()).filter(Boolean),
      isPublished: raw.isPublished,
    };

    const req$ = this.selectedId
      ? this.postsService.adminUpdate(this.selectedId, payload)
      : this.postsService.adminCreate(payload);

    req$.subscribe({
      next: () => {
        this.message = 'Saved.';
        this.reload();
      },
      complete: () => {
        this.saving = false;
      }
    });
  }

  remove() {
    if (!this.selectedId) return;

    this.postsService.adminDelete(this.selectedId).subscribe({
      next: () => {
        this.reset();
        this.reload();
      }
    });
  }

  private reload() {
    this.postsService.adminList({ pageNumber: 1, pageSize: 50 }).subscribe(r => {
      this.posts = r.items;
    });
  }
}
