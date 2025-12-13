export interface User {
  id?: number;
  name?: string;
  email: string;
  role: 'admin' | 'doutor';
  accessToken?: string;
}

export interface LoginResponse {
  accessToken: string;
  role: 'admin' | 'doutor';
}

