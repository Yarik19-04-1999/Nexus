export interface Universe {
  id: number
  name: string
  description?: string
  isHidden: boolean
  listNo: number
  createdAt: string
  updatedAt: string
}

export interface PagedResult<T> {
  items: T[]
  totalCount: number
  page: number
  pageSize: number
  totalPages: number
}

export interface DomainError {
  errorCode: string
  errorMessage?: string
  canRetry: boolean
}
