import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { HttpClient } from '@angular/common/http';
import { API_CONFIG } from '../config/api.config';
import { catchError } from 'rxjs/operators';
import { of } from 'rxjs';

export const doutorGuard: CanActivateFn = () => {
  const authService = inject(AuthService);
  const router = inject(Router);
  const http = inject(HttpClient);

  if (!authService.isAuthenticated() || !authService.isDoutor()) {
    return router.createUrlTree(['/login']);
  }

  http.get(`${API_CONFIG.baseUrl}/Auth/doutor/protected`)
    .pipe(
      catchError(() => {
        authService.logout();
        router.navigate(['/login']);
        return of(false);
      })
    )
    .subscribe();

  return true;
};

