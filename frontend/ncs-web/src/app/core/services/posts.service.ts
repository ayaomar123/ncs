import { Injectable } from '@angular/core';
import { ApiClient } from './api-client.service';
import { PagedResult } from '../models/paged-result';
import { BlogPost } from '../models/blog-post';

@Injectable({ providedIn: 'root' })
export class PostsService {
  constructor(private api: ApiClient) {}

  list(params?: { pageNumber?: number; pageSize?: number; tag?: string }) {
    return this.api.get<PagedResult<BlogPost>>('/api/posts', params);
  }

  getBySlug(slug: string) {
    return this.api.get<BlogPost>(`/api/posts/${encodeURIComponent(slug)}`);
  }

  adminList(params?: { pageNumber?: number; pageSize?: number; tag?: string }) {
    return this.api.get<PagedResult<BlogPost>>('/api/admin/posts', params);
  }

  adminGet(id: string) {
    return this.api.get<BlogPost>(`/api/admin/posts/${id}`);
  }

  adminCreate(payload: Partial<BlogPost>) {
    return this.api.post<BlogPost>('/api/admin/posts', payload);
  }

  adminUpdate(id: string, payload: Partial<BlogPost>) {
    return this.api.put<BlogPost>(`/api/admin/posts/${id}`, payload);
  }

  adminDelete(id: string) {
    return this.api.delete<void>(`/api/admin/posts/${id}`);
  }
}
