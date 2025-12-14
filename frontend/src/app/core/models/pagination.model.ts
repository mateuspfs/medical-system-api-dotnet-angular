export interface PaginatedResponse<T> {
  items: T[];
  totalPages: number;
  currentPage?: number;
  totalItems?: number;
}

export interface PaginationParams {
  pageNumber: number;
  pageSize: number;
}

