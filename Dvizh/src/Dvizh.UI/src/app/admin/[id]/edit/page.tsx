'use client'

import { use } from 'react'
import { InviteForm } from '@/components/admin/InviteForm'
import { useInviteById, useUpdateInvite } from '@/hooks/useInvites'
import { Spinner } from '@/components/ui/Spinner'
import { strings } from '@/lib/strings'
import type { UpdateInvitePayload } from '@/lib/api/invites'

interface Props {
  params: Promise<{ id: string }>
}

export default function EditInvitePage({ params }: Props) {
  const { id } = use(params)
  const inviteId = Number(id)

  const { data: invite, isPending: isLoading } = useInviteById(inviteId)
  const update = useUpdateInvite()

  if (isLoading) {
    return (
      <main className="min-h-screen bg-gray-50 flex items-center justify-center">
        <Spinner />
      </main>
    )
  }

  if (!invite) return null

  return (
    <main className="min-h-screen bg-gray-50 p-4 sm:p-8">
      <div className="max-w-lg mx-auto">
        <h1 className="text-xl font-bold text-gray-800 mb-6">{strings.admin.form.editTitle}</h1>
        <div className="bg-white rounded-2xl p-6 shadow-sm border border-gray-100">
          <InviteForm
            invite={invite}
            onSubmit={(payload) => update.mutateAsync(payload as UpdateInvitePayload)}
            isPending={update.isPending}
          />
        </div>
      </div>
    </main>
  )
}
