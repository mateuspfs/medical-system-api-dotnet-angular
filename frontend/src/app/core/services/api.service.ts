import { Injectable } from '@angular/core';
import axios, { AxiosInstance, AxiosRequestConfig } from 'axios';
import { Observable, from } from 'rxjs';
import { API_CONFIG } from '../config/api.config';
import { AuthService } from './auth.service';
import { ApiResponse } from '../models/api-response.model';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private axiosInstance: AxiosInstance;

  constructor(private authService: AuthService) {
    this.axiosInstance = axios.create({
      baseURL: API_CONFIG.baseUrl,
      headers: {
        'Content-Type': 'application/json'
      }
    });

    // Interceptor para adicionar token
    this.axiosInstance.interceptors.request.use(
      (config) => {
        const token = this.authService.getToken();
        if (token && config.headers) {
          config.headers.Authorization = `Bearer ${token}`;
        }
        return config;
      },
      (error) => {
        return Promise.reject(error);
      }
    );

    // Interceptor para tratamento de erros
    this.axiosInstance.interceptors.response.use(
      (response) => {
        const body = response.data;

        // Desembrulha o formato { success, data, message } vindo da API
        if (body && typeof body === 'object' && 'success' in body) {
          const apiResponse = body as ApiResponse<any>;
          if (apiResponse.success) {
            response.data = apiResponse.data as unknown;
            return response;
          }
          return Promise.reject(apiResponse);
        }

        return response;
      },
      (error) => {
        if (error.response?.status === 401 || error.response?.status === 403) {
          this.authService.logout();
          window.location.href = '/login';
        }
        return Promise.reject(error);
      }
    );
  }

  get<T>(url: string, config?: AxiosRequestConfig): Observable<T> {
    return from(this.axiosInstance.get<T>(url, config).then(response => response.data));
  }

  post<T>(url: string, data?: any, config?: AxiosRequestConfig): Observable<T> {
    return from(this.axiosInstance.post<T>(url, data, config).then(response => response.data));
  }

  put<T>(url: string, data?: any, config?: AxiosRequestConfig): Observable<T> {
    return from(this.axiosInstance.put<T>(url, data, config).then(response => response.data));
  }

  delete<T>(url: string, config?: AxiosRequestConfig): Observable<T> {
    return from(this.axiosInstance.delete<T>(url, config).then(response => response.data));
  }
}

