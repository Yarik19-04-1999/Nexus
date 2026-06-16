'use client'

import { useState, useEffect } from 'react'
import { X } from 'lucide-react'
import { useInviteEvents } from '@/hooks/useInvites'
import { Pagination } from './Pagination'
import { Spinner } from '@/components/ui/Spinner'
import { useAdminStrings } from './AdminLanguageContext'
import { InviteEventType } from '@/types/invite'
import { DEFAULT_PAGE_SIZE } from '@/lib/constants'
import type { InviteEventParams } from '@/lib/api/invites'

interface Props {
  inviteId: number
  inviteMessage: string
  open: boolean
  onClose: () => void
}

type TimeAgo = {
  justNow: string
  seconds: (n: number) => string
  minutes: (n: number) => string
  hours: (n: number) => string
  days: (n: number) => string
}

function parseUtc(dateString: string): Date {
  if (!dateString.endsWith('Z') && !/[+-]\d{2}:\d{2}$/.test(dateString)) {
    return new Date(dateString + 'Z')
  }
  return new Date(dateString)
}

function formatTimeAgo(dateString: string, t: TimeAgo): string {
  const diff = Math.floor((Date.now() - parseUtc(dateString).getTime()) / 1000)
  if (diff < 5) return t.justNow
  if (diff < 60) return t.seconds(diff)
  const minutes = Math.floor(diff / 60)
  if (minutes < 60) return t.minutes(minutes)
  const hours = Math.floor(minutes / 60)
  if (hours < 24) return t.hours(hours)
  return t.days(Math.floor(hours / 24))
}

const EVENT_BADGE_CLASS: Record<number, string> = {
  [InviteEventType.Opened]: 'bg-slate-100 text-slate-600',
  [InviteEventType.SaidYes]: 'bg-emerald-50 text-emerald-700',
  [InviteEventType.SaidNo]: 'bg-rose-50 text-rose-700',
  [InviteEventType.Reset]: 'bg-amber-50 text-amber-700',
}

export function InviteEventsModal({ inviteId, inviteMessage, open, onClose }: Props) {
  const { strings } = useAdminStrings()
  const s = strings.admin.events

  const [page, setPage] = useState(1)
  const [pageSize, setPageSize] = useState(DEFAULT_PAGE_SIZE)

  const params: InviteEventParams = { page, pageSize }
  const { data, isPending } = useInviteEvents(inviteId, params, open)

  useEffect(() => {
    if (open) {
      setPage(1)
    }
  }, [open])

  useEffect(() => {
    if (!open) return
    const handler = (e: KeyboardEvent) => {
      if (e.key === 'Escape') onClose()
    }
    window.addEventListener('keydown', handler)
    return () => window.removeEventListener('keydown', handler)
  }, [open, onClose])

  if (!open) return null

  return (
    <div
      className="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/40 backdrop-blur-sm"
      onClick={onClose}
    >
      <div
        className="w-full max-w-md bg-white rounded-2xl shadow-xl flex flex-col max-h-[80vh]"
        onClick={e => e.stopPropagation()}
      >
        {/* Header */}
        <div className="flex items-start justify-between gap-3 p-5 border-b border-gray-100">
          <div className="min-w-0">
            <h2 className="text-sm font-semibold text-gray-800">{s.title}</h2>
            <p className="text-xs text-gray-400 mt-0.5 truncate">{inviteMessage}</p>
          </div>
          <button
            onClick={onClose}
            className="shrink-0 p-1.5 rounded-lg text-gray-400 hover:text-gray-600 hover:bg-gray-100 transition-colors"
          >
            <X className="w-4 h-4" />
          </button>
        </div>

        {/* Body */}
        <div className="flex-1 overflow-y-auto">
          {isPending ? (
            <div className="flex justify-center py-12">
              <Spinner />
            </div>
          ) : !data?.items.length ? (
            <p className="text-center text-sm text-gray-400 py-12">{s.empty}</p>
          ) : (
            <table className="w-full text-sm">
              <tbody className="divide-y divide-gray-50">
                {data.items.map(event => (
                  <tr key={event.id} className="hover:bg-gray-50/50 transition-colors">
                    <td className="px-5 py-3">
                      <span className={`inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium ${EVENT_BADGE_CLASS[event.eventType] ?? 'bg-gray-100 text-gray-600'}`}>
                        {s.types[event.eventType] ?? String(event.eventType)}
                      </span>
                    </td>
                    <td className="px-5 py-3 text-xs text-gray-400 text-right">
                      {formatTimeAgo(event.createdAt, s.timeAgo)}
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          )}
        </div>

        {/* Footer */}
        {data && data.totalCount > 0 && (
          <div className="border-t border-gray-100 px-4 py-3">
            <Pagination
              page={page}
              pageSize={pageSize}
              totalPages={data.totalPages}
              totalCount={data.totalCount}
              onPageChange={setPage}
              onPageSizeChange={size => { setPageSize(size); setPage(1) }}
            />
          </div>
        )}
      </div>
    </div>
  )
}
