'use client'

import { useState } from 'react'
import { useRouter } from 'next/navigation'
import { Pencil, Trash2, RotateCcw, ExternalLink, Copy, Check } from 'lucide-react'
import { ConfirmDialog } from '@/components/ui/ConfirmDialog'
import { useDeleteInvite, useResetInviteAnswer } from '@/hooks/useInvites'
import { strings } from '@/lib/strings'
import type { Invite } from '@/types/invite'

interface InviteActionsProps {
  invite: Invite
}

export function InviteActions({ invite }: InviteActionsProps) {
  const router = useRouter()
  const [dialog, setDialog] = useState<'delete' | 'reset' | null>(null)
  const [copied, setCopied] = useState(false)

  const deleteInvite = useDeleteInvite()
  const resetAnswer = useResetInviteAnswer()

  const inviteUrl = `${window.location.origin}/${invite.code}`
  const s = strings.admin

  const handleCopy = async () => {
    await navigator.clipboard.writeText(inviteUrl)
    setCopied(true)
    setTimeout(() => setCopied(false), 2000)
  }

  const handleDelete = async () => {
    await deleteInvite.mutateAsync(invite.id)
    setDialog(null)
  }

  const handleReset = async () => {
    await resetAnswer.mutateAsync(invite.id)
    setDialog(null)
  }

  return (
    <>
      <div className="flex items-center gap-1 justify-end">
        <ActionButton onClick={handleCopy} title={s.actions.copy}>
          {copied ? <Check className="w-4 h-4 text-emerald-500" /> : <Copy className="w-4 h-4" />}
        </ActionButton>
        <ActionButton onClick={() => window.open(inviteUrl, '_blank')} title={s.actions.copy}>
          <ExternalLink className="w-4 h-4" />
        </ActionButton>
        <ActionButton onClick={() => router.push(`/admin/${invite.id}/edit`)} title={s.actions.edit}>
          <Pencil className="w-4 h-4" />
        </ActionButton>
        <ActionButton onClick={() => setDialog('reset')} title={s.actions.reset}>
          <RotateCcw className="w-4 h-4" />
        </ActionButton>
        <ActionButton onClick={() => setDialog('delete')} title={s.actions.delete} danger>
          <Trash2 className="w-4 h-4" />
        </ActionButton>
      </div>

      <ConfirmDialog
        open={dialog === 'delete'}
        title={s.confirmDelete.title}
        description={s.confirmDelete.description}
        confirmLabel={s.confirmDelete.confirm}
        cancelLabel={s.confirmDelete.cancel}
        isDanger
        isPending={deleteInvite.isPending}
        onConfirm={handleDelete}
        onCancel={() => setDialog(null)}
      />

      <ConfirmDialog
        open={dialog === 'reset'}
        title={s.confirmReset.title}
        description={s.confirmReset.description}
        confirmLabel={s.confirmReset.confirm}
        cancelLabel={s.confirmReset.cancel}
        isPending={resetAnswer.isPending}
        onConfirm={handleReset}
        onCancel={() => setDialog(null)}
      />
    </>
  )
}

function ActionButton({
  onClick,
  title,
  danger = false,
  children,
}: {
  onClick: () => void
  title: string
  danger?: boolean
  children: React.ReactNode
}) {
  return (
    <button
      onClick={onClick}
      title={title}
      className={`p-2 rounded-lg transition-colors
        ${danger ? 'text-gray-400 hover:text-rose-500 hover:bg-rose-50' : 'text-gray-400 hover:text-gray-700 hover:bg-gray-100'}`}
    >
      {children}
    </button>
  )
}
