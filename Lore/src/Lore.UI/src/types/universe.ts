import type { RewatchStatus } from './movie'

export interface Universe {
  id: number
  name: string
  description?: string
  isHidden: boolean
  listNo: number
  createdAt: string
  updatedAt: string
}

export interface MovieInUniverse {
  id: number
  title: string
  releaseYear: number
  durationMinutes: number
  reviewText: string | null
  score: number | null
  viewCount: number
  rewatchStatus: RewatchStatus
}

export interface UniverseDetail extends Universe {
  movies: MovieInUniverse[]
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
