import { Injectable } from '@angular/core';
import { ApiClient } from './api-client.service';
import { tap } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly tokenKey = 'ncs_admin_token';

  constructor(private api: ApiClient) {}

  login(email: string, password: string) {
    return this.api.post<{ token: string }>('/api/admin/auth/login', { email, password }).pipe(
      tap(r => localStorage.setItem(this.tokenKey, r.token)),
    );
  }

  logout() {
    localStorage.removeItem(this.tokenKey);
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }
}
