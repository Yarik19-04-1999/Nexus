'use client'

import { useState } from 'react'
import { useRouter } from 'next/navigation'
import { Pencil, Trash2, ExternalLink, Copy, Check, History } from 'lucide-react'
import { ConfirmDialog } from '@/components/ui/ConfirmDialog'
import { InviteEventsModal } from './InviteEventsModal'
import { useDeleteInvite } from '@/hooks/useInvites'
import { useAdminStrings } from './AdminLanguageContext'
import type { Invite } from '@/types/invite'

interface InviteActionsProps {
  invite: Invite
}

export function InviteActions({ invite }: InviteActionsProps) {
  const router = useRouter()
  const [showDeleteDialog, setShowDeleteDialog] = useState(false)
  const [showEventsModal, setShowEventsModal] = useState(false)
  const [copied, setCopied] = useState(false)
  const { strings } = useAdminStrings()

  const deleteInvite = useDeleteInvite()

  const inviteUrl = `${window.location.origin}/${invite.code}`
  const s = strings.admin

  const handleCopy = async () => {
    await navigator.clipboard.writeText(inviteUrl)
    setCopied(true)
    setTimeout(() => setCopied(false), 2000)
  }

  const handleDelete = async () => {
    await deleteInvite.mutateAsync(invite.id)
    setShowDeleteDialog(false)
  }

  return (
    <>
      <div className="flex items-center gap-1 justify-end">
        <ActionButton onClick={handleCopy} title={s.actions.copy}>
          {copied ? <Check className="w-4 h-4 text-emerald-500" /> : <Copy className="w-4 h-4" />}
        </ActionButton>
        <ActionButton onClick={() => window.open(inviteUrl, '_blank')} title={s.actions.open}>
          <ExternalLink className="w-4 h-4" />
        </ActionButton>
        <ActionButton onClick={() => setShowEventsModal(true)} title={s.actions.viewEvents}>
          <History className="w-4 h-4" />
        </ActionButton>
        <ActionButton onClick={() => router.push(`/admin/${invite.id}/edit`)} title={s.actions.edit}>
          <Pencil className="w-4 h-4" />
        </ActionButton>
        <ActionButton onClick={() => setShowDeleteDialog(true)} title={s.actions.delete} danger>
          <Trash2 className="w-4 h-4" />
        </ActionButton>
      </div>

      <InviteEventsModal
        inviteId={invite.id}
        inviteMessage={invite.message}
        open={showEventsModal}
        onClose={() => setShowEventsModal(false)}
      />

      <ConfirmDialog
        open={showDeleteDialog}
        title={s.confirmDelete.title}
        description={s.confirmDelete.description}
        confirmLabel={s.confirmDelete.confirm}
        cancelLabel={s.confirmDelete.cancel}
        isDanger
        isPending={deleteInvite.isPending}
        onConfirm={handleDelete}
        onCancel={() => setShowDeleteDialog(false)}
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
