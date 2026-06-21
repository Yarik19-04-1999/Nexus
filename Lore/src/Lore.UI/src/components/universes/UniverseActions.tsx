'use client'

import { useState } from 'react'
import { Pencil, Trash2 } from 'lucide-react'
import { ConfirmDialog } from '@/components/ui/ConfirmDialog'
import { useDeleteUniverse } from '@/hooks/useUniverses'
import type { Universe } from '@/types/universe'

interface UniverseActionsProps {
  universe: Universe
  onEdit: (universe: Universe) => void
}

export function UniverseActions({ universe, onEdit }: UniverseActionsProps) {
  const [showDelete, setShowDelete] = useState(false)
  const deleteUniverse = useDeleteUniverse()

  const handleDelete = async () => {
    await deleteUniverse.mutateAsync(universe.id)
    setShowDelete(false)
  }

  return (
    <>
      <div className="flex items-center gap-1 justify-end">
        <ActionButton onClick={() => onEdit(universe)} title="Edit">
          <Pencil className="w-4 h-4" />
        </ActionButton>
        <ActionButton onClick={() => setShowDelete(true)} title="Delete" danger>
          <Trash2 className="w-4 h-4" />
        </ActionButton>
      </div>

      <ConfirmDialog
        open={showDelete}
        title="Delete universe?"
        description="This action cannot be undone."
        confirmLabel="Delete"
        cancelLabel="Cancel"
        isDanger
        isPending={deleteUniverse.isPending}
        onConfirm={handleDelete}
        onCancel={() => setShowDelete(false)}
      />
    </>
  )
}

function ActionButton({ onClick, title, danger = false, children }: { onClick: () => void; title: string; danger?: boolean; children: React.ReactNode }) {
  return (
    <button
      onClick={onClick}
      title={title}
      className={`p-2 rounded-lg transition-colors ${danger ? 'text-gray-400 hover:text-rose-500 hover:bg-rose-50' : 'text-gray-400 hover:text-gray-700 hover:bg-gray-100'}`}
    >
      {children}
    </button>
  )
}
