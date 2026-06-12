'use client'

import { InviteForm } from '@/components/admin/InviteForm'
import { useCreateInvite } from '@/hooks/useInvites'
import { strings } from '@/lib/strings'

export default function NewInvitePage() {
  const create = useCreateInvite()

  return (
    <main className="min-h-screen bg-gray-50 p-4 sm:p-8">
      <div className="max-w-lg mx-auto">
        <h1 className="text-xl font-bold text-gray-800 mb-6">{strings.admin.form.createTitle}</h1>
        <div className="bg-white rounded-2xl p-6 shadow-sm border border-gray-100">
          <InviteForm
            onSubmit={(payload) => create.mutateAsync(payload as Parameters<typeof create.mutateAsync>[0])}
            isPending={create.isPending}
          />
        </div>
      </div>
    </main>
  )
}
