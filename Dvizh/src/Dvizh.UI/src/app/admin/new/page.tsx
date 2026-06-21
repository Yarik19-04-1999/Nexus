'use client'

import { InviteForm } from '@/components/admin/InviteForm'
import { useAdminStrings } from '@/components/admin/AdminLanguageContext'
import { useCreateInvite } from '@/hooks/useInvites'

export default function NewInvitePage() {
  const create = useCreateInvite()
  const { strings } = useAdminStrings()

  return (
    <main className="min-h-screen bg-gray-50 p-4 sm:p-8">
      <div className="max-w-lg mx-auto">
        <h1 className="text-xl font-bold text-gray-800 mb-6">{strings.admin.form.createTitle}</h1>
        <div className="bg-white rounded-2xl p-6 shadow-sm border border-gray-100">
          <InviteForm
            onSubmit={async (payload) => { await create.mutateAsync(payload as Parameters<typeof create.mutateAsync>[0]) }}
            isPending={create.isPending}
          />
        </div>
      </div>
    </main>
  )
}
