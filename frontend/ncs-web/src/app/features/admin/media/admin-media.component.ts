import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiClient } from '../../../core/services/api-client.service';

@Component({
  standalone: true,
  imports: [CommonModule],
  template: `
  <div class="container py-10">
    <h1 class="text-2xl font-semibold">Media upload</h1>
    <p class="mt-2 text-slate-700">Uploads are stored locally under <code>wwwroot/uploads</code> in the API.</p>

    <div class="mt-8 rounded border border-slate-200 bg-white p-5">
      <label class="block">
        <span class="text-sm font-medium">Choose an image</span>
        <input class="mt-2 block" type="file" accept="image/*" (change)="onFile($event)" />
      </label>

      <div class="mt-4">
        <button class="rounded bg-slate-900 px-4 py-2 text-white hover:bg-slate-800" (click)="upload()" [disabled]="!file || uploading">
          {{ uploading ? 'Uploading…' : 'Upload' }}
        </button>
      </div>

      <div class="mt-4 text-sm text-emerald-700" *ngIf="url">Uploaded: {{ url }}</div>
    </div>
  </div>
  `,
})
export class AdminMediaComponent {
  file: File | null = null;
  uploading = false;
  url = '';

  constructor(private api: ApiClient) {}

  onFile(event: Event) {
    const input = event.target as HTMLInputElement;
    this.file = input.files?.item(0) ?? null;
  }

  upload() {
    if (!this.file) return;

    this.uploading = true;
    this.url = '';

    const formData = new FormData();
    formData.append('file', this.file);

    this.api.postFormData<{ url: string }>('/api/admin/media/upload', formData).subscribe({
      next: r => {
        this.url = r.url;
      },
      complete: () => {
        this.uploading = false;
      }
    });
  }
}
