import { Component, OnInit, AfterViewInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { API_CONFIG } from '../../../core/config/api.config';

declare var google: any;

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styles: []
})
export class LoginComponent implements OnInit, AfterViewInit {
  loading = false;
  error = '';

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    if (this.authService.isAuthenticated()) {
      const role = this.authService.getUserRole();
      if (role === 'admin') {
        this.router.navigate(['/admin']);
      } else if (role === 'doutor') {
        this.router.navigate(['/doutor']);
      }
    }
  }

  ngAfterViewInit(): void {
    google.accounts.id.initialize({
      client_id: API_CONFIG.googleClientId,
      callback: this.handleCredentialResponse.bind(this)
    });

    google.accounts.id.renderButton(
      document.getElementById('buttonDiv'),
      {
        type: 'standard',
        shape: 'rectangular',
        theme: 'filled_blue',
        text: 'signin',
        size: 'large',
        logo_alignment: 'left',
        width: '300'
      }
    );

    google.accounts.id.prompt();
  }

  handleCredentialResponse(response: any): void {
    this.loading = true;
    this.error = '';

    this.authService.login(response.credential).subscribe({
      next: (loginResponse) => {
        this.loading = false;
        if (loginResponse.role === 'admin') {
          this.router.navigate(['/admin']);
        } else if (loginResponse.role === 'doutor') {
          this.router.navigate(['/doutor']);
        }
      },
      error: (error) => {
        this.loading = false;
        this.error = 'Não autorizado! Verifique suas credenciais.';
        console.error('Erro no login:', error);
      }
    });
  }
}

