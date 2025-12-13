import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { tap } from 'rxjs/operators';
import { API_CONFIG } from '../config/api.config';
import { User, LoginResponse } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private currentUserSubject = new BehaviorSubject<User | null>(this.getUserFromStorage());
  public currentUser$ = this.currentUserSubject.asObservable();

  constructor(private http: HttpClient) {}

  login(googleToken: string): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${API_CONFIG.baseUrl}/Auth/login?token=${googleToken}`, {})
      .pipe(
        tap(response => {
          localStorage.setItem('accessToken', response.accessToken);
          const user: User = {
            email: '',
            role: response.role,
            accessToken: response.accessToken
          };
          localStorage.setItem('userRole', response.role);
          this.currentUserSubject.next(user);
        })
      );
  }

  logout(): void {
    localStorage.removeItem('accessToken');
    localStorage.removeItem('userRole');
    this.currentUserSubject.next(null);
  }

  getToken(): string | null {
    return localStorage.getItem('accessToken');
  }

  getUserRole(): string | null {
    return localStorage.getItem('userRole');
  }

  isAuthenticated(): boolean {
    return !!this.getToken();
  }

  isAdmin(): boolean {
    return this.getUserRole() === 'admin';
  }

  isDoutor(): boolean {
    return this.getUserRole() === 'doutor';
  }

  private getUserFromStorage(): User | null {
    const token = this.getToken();
    const role = this.getUserRole();
    if (token && role) {
      return {
        email: '',
        role: role as 'admin' | 'doutor',
        accessToken: token
      };
    }
    return null;
  }
}

