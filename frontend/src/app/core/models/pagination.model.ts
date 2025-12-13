export interface PaginatedResponse<T> {
  items: {
    $values: T[];
  };
  totalPages: number;
  currentPage: number;
  totalItems: number;
}

export interface PaginationParams {
  pageNumber: number;
  pageSize: number;
}

