import { apiClient } from './client'
import type { Invite, InviteEvent, PagedResult } from '@/types/invite'
import { InviteAnswer, InviteLanguage, InviteMascot } from '@/types/invite'

export interface CreateInvitePayload {
  message: string
  description?: string
  expiresAt?: string
  language: InviteLanguage
  mascot: InviteMascot
}

export interface UpdateInvitePayload {
  id: number
  message: string
  description?: string
  expiresAt?: string
  language: InviteLanguage
  mascot: InviteMascot
}

export interface InviteEventParams {
  page: number
  pageSize: number
}

function buildEventsQuery(params: InviteEventParams): string {
  const q = new URLSearchParams()
  q.set('page', String(params.page))
  q.set('pageSize', String(params.pageSize))
  q.set('sorts', '-CreatedAt')
  return q.toString()
}

export interface InviteListParams {
  page: number
  pageSize: number
  q?: string
  answer?: string
  expiry?: 'expired' | 'active'
  sort?: string
  dir?: 'asc' | 'desc'
}

function buildQuery(params: InviteListParams): string {
  const q = new URLSearchParams()
  q.set('page', String(params.page))
  q.set('pageSize', String(params.pageSize))

  const filters: string[] = []
  if (params.q) filters.push(`Message@=*${params.q}`)
  if (params.answer !== undefined) filters.push(`Answer==${params.answer}`)
  if (params.expiry) {
    const now = new Date().toISOString()
    if (params.expiry === 'active') filters.push(`expiresAt>=${now}`)
    else if (params.expiry === 'expired') filters.push(`expiresAt<${now}`)
  }
  if (filters.length) q.set('filters', filters.join(','))

  const SORT_FIELDS: Record<string, string> = {
    answer: 'Answer',
    expiresAt: 'ExpiresAt',
    createdAt: 'CreatedAt',
  }
  const sieveField = params.sort ? (SORT_FIELDS[params.sort] ?? 'CreatedAt') : 'CreatedAt'
  const prefix = (params.sort ? params.dir === 'desc' : true) ? '-' : ''
  q.set('sorts', `${prefix}${sieveField}`)

  return q.toString()
}

export const invitesApi = {
  getAll: (params: InviteListParams) =>
    apiClient.get<PagedResult<Invite>>(`/api/v1/invites?${buildQuery(params)}`),

  getById: (id: number) =>
    apiClient.get<Invite>(`/api/v1/invites/${id}`),

  open: (code: string) =>
    apiClient.get<Invite>(`/api/v1/invites/${code}`),

  create: (payload: CreateInvitePayload) =>
    apiClient.post<Invite>('/api/v1/invites', payload),

  update: (payload: UpdateInvitePayload) =>
    apiClient.put<Invite>('/api/v1/invites', payload),

  delete: (id: number) =>
    apiClient.delete(`/api/v1/invites/${id}`),

  reset: (id: number) =>
    apiClient.post<void>(`/api/v1/invites/${id}/answer/reset`),

  respond: (code: string, answer: InviteAnswer) =>
    apiClient.post<void>('/api/v1/invites/answer', { code, answer }),

  getEvents: (inviteId: number, params: InviteEventParams) =>
    apiClient.get<PagedResult<InviteEvent>>(`/api/v1/invites/${inviteId}/events?${buildEventsQuery(params)}`),
}
