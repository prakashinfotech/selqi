export interface RegisterRequest {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface UserDto {
  id: string;
  email: string;
  firstName: string;
  lastName: string;
  role: string;
}

export interface AuthResponse {
  token: string;
  expiresAt: string;
  user: UserDto;
}

export interface SavedAddress {
  id: string;
  label: string;
  firstName: string;
  lastName: string;
  line1: string;
  line2?: string;
  town: string;
  county?: string;
  postcode: string;
  country: string;
  phone: string;
  isDefault: boolean;
}

export interface SaveAddressRequest {
  label: string;
  firstName: string;
  lastName: string;
  line1: string;
  line2?: string | null;
  town: string;
  county?: string | null;
  postcode: string;
  country?: string;
  phone?: string | null;
  isDefault: boolean;
}
