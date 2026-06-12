'use client'

import { useState } from 'react'
import Link from 'next/link'
import { Plus } from 'lucide-react'
import { useInvitesList } from '@/hooks/useInvites'
import { InviteActions } from './InviteActions'
import { AnswerBadge } from './AnswerBadge'
import { Pagination } from './Pagination'
import { Spinner } from '@/components/ui/Spinner'
import { strings } from '@/lib/strings'
import { DEFAULT_PAGE_SIZE } from '@/lib/constants'

const s = strings.admin

export function InvitesTable() {
  const [page, setPage] = useState(1)
  const { data, isPending } = useInvitesList(page, DEFAULT_PAGE_SIZE)

  return (
    <div className="flex flex-col gap-4">
      <div className="flex items-center justify-between">
        <h1 className="text-xl font-bold text-gray-800">{s.title}</h1>
        <Link
          href="/admin/new"
          className="flex items-center gap-2 px-4 py-2 rounded-xl text-sm font-medium text-white bg-emerald-500 hover:bg-emerald-600 transition-colors"
        >
          <Plus className="w-4 h-4" />
          {s.newInvite}
        </Link>
      </div>

      {isPending ? (
        <div className="flex justify-center py-12"><Spinner /></div>
      ) : !data?.items.length ? (
        <p className="text-center text-gray-400 py-12">{s.noInvites}</p>
      ) : (
        <>
          <div className="overflow-x-auto rounded-2xl border border-gray-100">
            <table className="w-full text-sm">
              <thead>
                <tr className="bg-gray-50 text-left text-gray-500 text-xs uppercase tracking-wide">
                  <th className="px-4 py-3 font-medium">{s.columns.message}</th>
                  <th className="px-4 py-3 font-medium hidden sm:table-cell">{s.columns.answer}</th>
                  <th className="px-4 py-3 font-medium hidden md:table-cell">{s.columns.expiresAt}</th>
                  <th className="px-4 py-3 font-medium hidden lg:table-cell">{s.columns.createdAt}</th>
                  <th className="px-4 py-3" />
                </tr>
              </thead>
              <tbody className="divide-y divide-gray-50">
                {data.items.map((invite) => (
                  <tr key={invite.id} className="hover:bg-gray-50/50 transition-colors">
                    <td className="px-4 py-3">
                      <p className="font-medium text-gray-800 truncate max-w-[200px]">{invite.message}</p>
                      <p className="text-xs text-gray-400 font-mono mt-0.5">{invite.code}</p>
                    </td>
                    <td className="px-4 py-3 hidden sm:table-cell">
                      <AnswerBadge answer={invite.answer} />
                    </td>
                    <td className="px-4 py-3 text-gray-500 hidden md:table-cell">
                      {invite.expiresAt ? new Date(invite.expiresAt).toLocaleDateString() : '—'}
                    </td>
                    <td className="px-4 py-3 text-gray-500 hidden lg:table-cell">
                      {new Date(invite.createdAt).toLocaleDateString()}
                    </td>
                    <td className="px-4 py-3">
                      <InviteActions invite={invite} />
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>

          <Pagination
            page={page}
            totalPages={data.totalPages}
            onPageChange={setPage}
          />
        </>
      )}
    </div>
  )
}
