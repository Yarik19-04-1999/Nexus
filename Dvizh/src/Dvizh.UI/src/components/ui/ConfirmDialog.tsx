'use client'

import { Dialog, DialogPanel, DialogTitle } from '@headlessui/react'

interface ConfirmDialogProps {
  open: boolean
  title: string
  description: string
  confirmLabel: string
  cancelLabel: string
  isDanger?: boolean
  isPending?: boolean
  onConfirm: () => void
  onCancel: () => void
}

export function ConfirmDialog({
  open,
  title,
  description,
  confirmLabel,
  cancelLabel,
  isDanger = false,
  isPending = false,
  onConfirm,
  onCancel,
}: ConfirmDialogProps) {
  return (
    <Dialog open={open} onClose={onCancel} className="relative z-50">
      <div className="fixed inset-0 bg-black/25" aria-hidden="true" />
      <div className="fixed inset-0 flex items-center justify-center p-4">
        <DialogPanel className="w-full max-w-sm rounded-2xl bg-white p-6 shadow-xl">
          <DialogTitle className="text-base font-semibold text-gray-900">{title}</DialogTitle>
          <p className="mt-2 text-sm text-gray-500">{description}</p>
          <div className="mt-6 flex gap-3 justify-end">
            <button
              onClick={onCancel}
              disabled={isPending}
              className="px-4 py-2 rounded-xl text-sm text-gray-600 hover:bg-gray-100 transition-colors disabled:opacity-50"
            >
              {cancelLabel}
            </button>
            <button
              onClick={onConfirm}
              disabled={isPending}
              className={`px-4 py-2 rounded-xl text-sm font-medium text-white transition-colors disabled:opacity-50
                ${isDanger ? 'bg-rose-500 hover:bg-rose-600' : 'bg-emerald-500 hover:bg-emerald-600'}`}
            >
              {confirmLabel}
            </button>
          </div>
        </DialogPanel>
      </div>
    </Dialog>
  )
}
