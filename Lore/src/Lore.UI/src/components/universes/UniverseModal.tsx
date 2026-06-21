'use client'

import { Dialog, DialogPanel, DialogTitle } from '@headlessui/react'
import { X } from 'lucide-react'
import { UniverseForm } from './UniverseForm'
import type { Universe } from '@/types/universe'
import type { CreateUniversePayload, UpdateUniversePayload } from '@/lib/api/universes'

interface UniverseModalProps {
  open: boolean
  universe?: Universe | null
  isPending: boolean
  onClose: () => void
  onSubmit: (payload: CreateUniversePayload | UpdateUniversePayload) => Promise<void>
}

export function UniverseModal({ open, universe, isPending, onClose, onSubmit }: UniverseModalProps) {
  return (
    <Dialog open={open} onClose={onClose} className="relative z-50">
      <div className="fixed inset-0 bg-black/30" aria-hidden="true" />
      <div className="fixed inset-0 flex items-center justify-center p-4">
        <DialogPanel className="w-full max-w-lg rounded-2xl bg-white p-6 shadow-xl">
          <div className="flex items-center justify-between mb-5">
            <DialogTitle className="text-base font-semibold text-gray-900">
              {universe ? 'Edit universe' : 'New universe'}
            </DialogTitle>
            <button onClick={onClose} className="p-1.5 rounded-lg text-gray-400 hover:text-gray-600 hover:bg-gray-100 transition-colors">
              <X className="w-4 h-4" />
            </button>
          </div>
          <UniverseForm
            universe={universe ?? undefined}
            isPending={isPending}
            onSubmit={onSubmit}
            onCancel={onClose}
          />
        </DialogPanel>
      </div>
    </Dialog>
  )
}
