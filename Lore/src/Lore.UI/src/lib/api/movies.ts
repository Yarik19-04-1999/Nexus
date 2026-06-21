import { apiClient } from './client'
import type { Movie, SearchMovieResult } from '@/types/movie'
import type { PagedResult } from '@/types/universe'

export interface MovieListParams {
  page: number
  pageSize: number
  sorts?: string
  filters?: string
}

export interface CreateMoviePayload {
  title: string
  releaseYear: number
  durationMinutes: number
  reviewText?: string | null
  score?: number | null
  viewCount: number
  rewatchStatus: number
  universeId?: number | null
  listNo: number
}

export type UpdateMoviePayload = CreateMoviePayload

export const moviesApi = {
  getAll: (params: MovieListParams) => {
    const q = new URLSearchParams()
    q.set('page', String(params.page))
    q.set('pageSize', String(params.pageSize))
    if (params.sorts) q.set('sorts', params.sorts)
    if (params.filters) q.set('filters', params.filters)
    return apiClient.get<PagedResult<Movie>>(`/api/v1/movies?${q}`)
  },
  getById: (id: number) => apiClient.get<Movie>(`/api/v1/movies/${id}`),
  create: (payload: CreateMoviePayload) => apiClient.post<Movie>('/api/v1/movies', payload),
  update: (id: number, payload: UpdateMoviePayload) =>
    apiClient.put<Movie>(`/api/v1/movies/${id}`, payload),
  delete: (id: number) => apiClient.delete(`/api/v1/movies/${id}`),
  incrementViewCount: (id: number) =>
    apiClient.post<{ viewCount: number }>(`/api/v1/movies/${id}/view`),
  decrementViewCount: (id: number) =>
    apiClient.delete<{ viewCount: number }>(`/api/v1/movies/${id}/view`),
  link: (movieId: number, universeId: number) =>
    apiClient.post('/api/v1/movies/link', { movieId, universeId }),
  unlink: (movieId: number) =>
    apiClient.post('/api/v1/movies/unlink', { movieId }),
  search: (q: string) =>
    apiClient.get<SearchMovieResult[]>(`/api/v1/movies/search?q=${encodeURIComponent(q)}`),
}
