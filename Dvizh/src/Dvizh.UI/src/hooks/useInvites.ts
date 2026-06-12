'use client'

import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query'
import { invitesApi, type CreateInvitePayload, type UpdateInvitePayload } from '@/lib/api/invites'
import { DEFAULT_PAGE_SIZE } from '@/lib/constants'
import { InviteAnswer } from '@/types/invite'

export const inviteKeys = {
  all: ['invites'] as const,
  list: (page: number, pageSize: number) => [...inviteKeys.all, 'list', page, pageSize] as const,
  detail: (id: number) => [...inviteKeys.all, 'detail', id] as const,
  open: (code: string) => [...inviteKeys.all, 'open', code] as const,
}

export function useInvitesList(page: number, pageSize = DEFAULT_PAGE_SIZE) {
  return useQuery({
    queryKey: inviteKeys.list(page, pageSize),
    queryFn: () => invitesApi.getAll(page, pageSize),
  })
}

export function useOpenInvite(code: string) {
  return useQuery({
    queryKey: inviteKeys.open(code),
    queryFn: () => invitesApi.open(code),
    retry: false,
  })
}

export function useInviteById(id: number) {
  return useQuery({
    queryKey: inviteKeys.detail(id),
    queryFn: () => invitesApi.getById(id),
  })
}

export function useCreateInvite() {
  const queryClient = useQueryClient()
  return useMutation({
    mutationFn: (payload: CreateInvitePayload) => invitesApi.create(payload),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: inviteKeys.all }),
  })
}

export function useUpdateInvite() {
  const queryClient = useQueryClient()
  return useMutation({
    mutationFn: (payload: UpdateInvitePayload) => invitesApi.update(payload),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: inviteKeys.all }),
  })
}

export function useDeleteInvite() {
  const queryClient = useQueryClient()
  return useMutation({
    mutationFn: (id: number) => invitesApi.delete(id),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: inviteKeys.all }),
  })
}

export function useResetInviteAnswer() {
  const queryClient = useQueryClient()
  return useMutation({
    mutationFn: (id: number) => invitesApi.reset(id),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: inviteKeys.all }),
  })
}

export function useRespondToInvite(code: string) {
  const queryClient = useQueryClient()
  return useMutation({
    mutationFn: (answer: InviteAnswer) => invitesApi.respond(code, answer),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: inviteKeys.open(code) }),
  })
}

export function useResetAndRefetch(code: string) {
  const queryClient = useQueryClient()
  return useMutation({
    mutationFn: (id: number) => invitesApi.reset(id),
    onSuccess: () => queryClient.invalidateQueries({ queryKey: inviteKeys.open(code) }),
  })
}
