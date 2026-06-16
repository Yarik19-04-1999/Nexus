import { apiClient } from './client'
import type { Invite, PagedResult } from '@/types/invite'
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

export const invitesApi = {
  getAll: (page: number, pageSize: number) =>
    apiClient.get<PagedResult<Invite>>(`/api/v1/invites?page=${page}&pageSize=${pageSize}`),

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
}
