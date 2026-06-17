'use client'

import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { universesApi, type CreateUniversePayload, type UpdateUniversePayload, type UniverseListParams } from '@/lib/api/universes'

export const universeKeys = {
  all: ['universes'] as const,
  list: (params: UniverseListParams) => [...universeKeys.all, 'list', params] as const,
  detail: (id: number) => [...universeKeys.all, 'detail', id] as const,
}

export function useUniversesList(params: UniverseListParams) {
  return useQuery({
    queryKey: universeKeys.list(params),
    queryFn: () => universesApi.getAll(params),
  })
}

export function useUniverseById(id: number) {
  return useQuery({
    queryKey: universeKeys.detail(id),
    queryFn: () => universesApi.getById(id),
  })
}

export function useCreateUniverse() {
  const queryClient = useQueryClient()
  return useMutation({
    mutationFn: (payload: CreateUniversePayload) => universesApi.create(payload),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: universeKeys.all }),
  })
}

export function useUpdateUniverse() {
  const queryClient = useQueryClient()
  return useMutation({
    mutationFn: (payload: UpdateUniversePayload) => universesApi.update(payload),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: universeKeys.all }),
  })
}

export function useDeleteUniverse() {
  const queryClient = useQueryClient()
  return useMutation({
    mutationFn: (id: number) => universesApi.delete(id),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: universeKeys.all }),
  })
}
