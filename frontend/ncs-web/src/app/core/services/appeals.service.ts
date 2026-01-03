import { Injectable } from '@angular/core';
import { ApiClient } from './api-client.service';
import { PagedResult } from '../models/paged-result';
import { Appeal } from '../models/appeal';

@Injectable({ providedIn: 'root' })
export class AppealsService {
  constructor(private api: ApiClient) {}

  list(params?: { pageNumber?: number; pageSize?: number; isUrgent?: boolean; countryTag?: string }) {
    return this.api.get<PagedResult<Appeal>>('/api/appeals', params);
  }

  getBySlug(slug: string) {
    return this.api.get<Appeal>(`/api/appeals/${encodeURIComponent(slug)}`);
  }

  adminList(params?: { pageNumber?: number; pageSize?: number; isUrgent?: boolean; countryTag?: string }) {
    return this.api.get<PagedResult<Appeal>>('/api/admin/appeals', params);
  }

  adminGet(id: string) {
    return this.api.get<Appeal>(`/api/admin/appeals/${id}`);
  }

  adminCreate(payload: Partial<Appeal>) {
    return this.api.post<Appeal>('/api/admin/appeals', payload);
  }

  adminUpdate(id: string, payload: Partial<Appeal>) {
    return this.api.put<Appeal>(`/api/admin/appeals/${id}`, payload);
  }

  adminDelete(id: string) {
    return this.api.delete<void>(`/api/admin/appeals/${id}`);
  }
}
