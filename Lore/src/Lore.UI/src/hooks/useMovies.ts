'use client'

import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { moviesApi, type MovieListParams, type CreateMoviePayload, type UpdateMoviePayload } from '@/lib/api/movies'

export const movieKeys = {
  all: ['movies'] as const,
  list: (params: MovieListParams) => [...movieKeys.all, 'list', params] as const,
  detail: (id: number) => [...movieKeys.all, 'detail', id] as const,
  search: (q: string) => [...movieKeys.all, 'search', q] as const,
}

export function useMoviesList(params: MovieListParams) {
  return useQuery({
    queryKey: movieKeys.list(params),
    queryFn: () => moviesApi.getAll(params),
  })
}

export function useSearchMovies(q: string) {
  return useQuery({
    queryKey: movieKeys.search(q),
    queryFn: () => moviesApi.search(q),
    enabled: q.trim().length >= 3,
  })
}

export function useCreateMovie() {
  const qc = useQueryClient()
  return useMutation({
    mutationFn: (payload: CreateMoviePayload) => moviesApi.create(payload),
    onSuccess: () => qc.invalidateQueries({ queryKey: movieKeys.all }),
  })
}

export function useUpdateMovie() {
  const qc = useQueryClient()
  return useMutation({
    mutationFn: ({ id, payload }: { id: number; payload: UpdateMoviePayload }) =>
      moviesApi.update(id, payload),
    onSuccess: () => qc.invalidateQueries({ queryKey: movieKeys.all }),
  })
}

export function useDeleteMovie() {
  const qc = useQueryClient()
  return useMutation({
    mutationFn: (id: number) => moviesApi.delete(id),
    onSuccess: () => qc.invalidateQueries({ queryKey: movieKeys.all }),
  })
}

export function useIncrementViewCount() {
  const qc = useQueryClient()
  return useMutation({
    mutationFn: (id: number) => moviesApi.incrementViewCount(id),
    onSuccess: () => qc.invalidateQueries({ queryKey: movieKeys.all }),
  })
}

export function useDecrementViewCount() {
  const qc = useQueryClient()
  return useMutation({
    mutationFn: (id: number) => moviesApi.decrementViewCount(id),
    onSuccess: () => qc.invalidateQueries({ queryKey: movieKeys.all }),
  })
}

export function useLinkMovie() {
  const qc = useQueryClient()
  return useMutation({
    mutationFn: ({ movieId, universeId }: { movieId: number; universeId: number }) =>
      moviesApi.link(movieId, universeId),
    onSuccess: () => qc.invalidateQueries({ queryKey: movieKeys.all }),
  })
}

export function useUnlinkMovie() {
  const qc = useQueryClient()
  return useMutation({
    mutationFn: (movieId: number) => moviesApi.unlink(movieId),
    onSuccess: () => qc.invalidateQueries({ queryKey: movieKeys.all }),
  })
}
