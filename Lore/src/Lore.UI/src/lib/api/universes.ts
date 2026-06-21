import { apiClient } from './client'
import type { Universe, UniverseDetail, PagedResult } from '@/types/universe'

export interface UniverseListParams {
  page: number
  pageSize: number
  q?: string
  sort?: string
  dir?: 'asc' | 'desc'
}

export interface CreateUniversePayload {
  name: string
  description?: string
  isHidden: boolean
  listNo: number
}

export interface UpdateUniversePayload {
  id: number
  name: string
  description?: string
  isHidden: boolean
  listNo: number
}

function buildQuery(params: UniverseListParams): string {
  const q = new URLSearchParams()
  q.set('page', String(params.page))
  q.set('pageSize', String(params.pageSize))

  if (params.q) {
    q.set('filters', `Name@=*${params.q}`)
  }

  const SORT_FIELDS: Record<string, string> = {
    name: 'Name',
    listNo: 'ListNo',
    createdAt: 'CreatedAt',
  }
  const sieveField = params.sort ? (SORT_FIELDS[params.sort] ?? 'ListNo') : 'ListNo'
  const prefix = params.sort ? (params.dir === 'desc' ? '-' : '') : ''
  q.set('sorts', `${prefix}${sieveField}`)

  return q.toString()
}

export const universesApi = {
  getAll: (params: UniverseListParams) =>
    apiClient.get<PagedResult<Universe>>(`/api/v1/universes?${buildQuery(params)}`),

  getById: (id: number) =>
    apiClient.get<UniverseDetail>(`/api/v1/universes/${id}`),

  create: (payload: CreateUniversePayload) =>
    apiClient.post<Universe>('/api/v1/universes', payload),

  update: (payload: UpdateUniversePayload) =>
    apiClient.put<Universe>('/api/v1/universes', payload),

  delete: (id: number) =>
    apiClient.delete(`/api/v1/universes/${id}`),

  search: (q: string) =>
    apiClient.get<Array<{ id: number; name: string }>>(`/api/v1/universes/search?q=${encodeURIComponent(q)}`),
}
