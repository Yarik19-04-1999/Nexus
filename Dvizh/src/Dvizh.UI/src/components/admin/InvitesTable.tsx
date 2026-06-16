'use client'

import { useState, useEffect, useCallback } from 'react'
import Link from 'next/link'
import { useRouter, useSearchParams, usePathname } from 'next/navigation'
import { Plus, Search, ChevronUp, ChevronDown, ChevronsUpDown } from 'lucide-react'
import { useInvitesList } from '@/hooks/useInvites'
import { InviteActions } from './InviteActions'
import { AnswerBadge } from './AnswerBadge'
import { Pagination } from './Pagination'
import { LanguageSelector } from './LanguageSelector'
import { Spinner } from '@/components/ui/Spinner'
import { useAdminStrings } from './AdminLanguageContext'
import { DEFAULT_PAGE_SIZE } from '@/lib/constants'
import type { InviteListParams } from '@/lib/api/invites'

type SortDir = 'asc' | 'desc'

function pill(active: boolean) {
  return `px-3 py-1 rounded-lg text-xs font-medium border transition-colors ${
    active
      ? 'bg-emerald-50 border-emerald-300 text-emerald-700'
      : 'bg-white border-gray-200 text-gray-500 hover:border-gray-300'
  }`
}

function SortIcon({ field, sort, dir }: { field: string; sort: string | null; dir: SortDir }) {
  if (sort !== field) return <ChevronsUpDown className="w-3 h-3 text-gray-300" />
  return dir === 'asc'
    ? <ChevronUp className="w-3 h-3 text-emerald-500" />
    : <ChevronDown className="w-3 h-3 text-emerald-500" />
}

export function InvitesTable() {
  const router = useRouter()
  const pathname = usePathname()
  const searchParams = useSearchParams()
  const { strings } = useAdminStrings()
  const s = strings.admin
  const f = s.filters

  // Read URL state
  const urlQ = searchParams.get('q') ?? ''
  const answer = searchParams.get('answer')
  const expiry = searchParams.get('expiry') as 'expired' | 'active' | null
  const sort = searchParams.get('sort')
  const dir = (searchParams.get('dir') ?? 'desc') as SortDir
  const page = Number(searchParams.get('page') ?? '1')
  const pageSize = Number(searchParams.get('pageSize') ?? String(DEFAULT_PAGE_SIZE))

  // Local state for debounced search input
  const [localQ, setLocalQ] = useState(urlQ)
  useEffect(() => { setLocalQ(urlQ) }, [urlQ])

  const updateParams = useCallback((updates: Record<string, string | null>) => {
    const params = new URLSearchParams(searchParams.toString())
    Object.entries(updates).forEach(([k, v]) => {
      if (v === null) params.delete(k)
      else params.set(k, v)
    })
    if (params.get('page') === '1') params.delete('page')
    router.replace(`${pathname}?${params.toString()}`, { scroll: false })
  }, [searchParams, pathname, router])

  // Debounce search to URL
  useEffect(() => {
    const timer = setTimeout(() => {
      if (localQ !== urlQ) {
        updateParams({ q: localQ || null, page: '1' })
      }
    }, 300)
    return () => clearTimeout(timer)
  }, [localQ]) // eslint-disable-line react-hooks/exhaustive-deps

  function handleSort(field: string) {
    if (sort === field) {
      updateParams({ sort: field, dir: dir === 'asc' ? 'desc' : 'asc' })
    } else {
      updateParams({ sort: field, dir: 'desc' })
    }
  }

  function handleAnswer(value: string | null) {
    updateParams({ answer: value, page: '1' })
  }

  function handleExpiry(value: 'expired' | 'active' | null) {
    updateParams({ expiry: value, page: '1' })
  }

  function handlePage(p: number) {
    updateParams({ page: String(p) })
  }

  function handlePageSize(size: number) {
    updateParams({ pageSize: String(size), page: '1' })
  }

  const listParams: InviteListParams = {
    page,
    pageSize,
    q: urlQ || undefined,
    answer: answer ?? undefined,
    expiry: expiry ?? undefined,
    sort: sort ?? undefined,
    dir,
  }

  const { data, isPending } = useInvitesList(listParams)

  const thSort = 'px-4 py-3 font-medium cursor-pointer select-none'
  const th = 'px-4 py-3 font-medium'

  return (
    <div className="flex flex-col gap-4">
      {/* Header */}
      <div className="flex items-center justify-between gap-3">
        <h1 className="text-xl font-bold text-gray-800">{s.title}</h1>
        <div className="flex items-center gap-3">
          <LanguageSelector />
          <Link
            href="/admin/new"
            className="flex items-center gap-2 px-4 py-2 rounded-xl text-sm font-medium text-white bg-emerald-500 hover:bg-emerald-600 transition-colors"
          >
            <Plus className="w-4 h-4" />
            {s.newInvite}
          </Link>
        </div>
      </div>

      {/* Filter bar */}
      <div className="flex flex-col gap-2">
        <div className="relative">
          <Search className="absolute left-3 top-1/2 -translate-y-1/2 w-3.5 h-3.5 text-gray-400" />
          <input
            type="text"
            value={localQ}
            onChange={e => setLocalQ(e.target.value)}
            placeholder={f.searchPlaceholder}
            className="w-full pl-9 pr-3 py-2 rounded-xl border border-gray-200 text-sm focus:outline-none focus:ring-2 focus:ring-emerald-300"
          />
        </div>

        <div className="flex flex-wrap items-center gap-4">
          <div className="flex gap-1">
            <button onClick={() => handleAnswer(null)} className={pill(answer === null)}>{f.answerAll}</button>
            <button onClick={() => handleAnswer('0')} className={pill(answer === '0')}>{f.answerPending}</button>
            <button onClick={() => handleAnswer('1')} className={pill(answer === '1')}>{f.answerYes}</button>
            <button onClick={() => handleAnswer('2')} className={pill(answer === '2')}>{f.answerNo}</button>
          </div>

          <div className="flex gap-1">
            <button onClick={() => handleExpiry(null)} className={pill(expiry === null)}>{f.expiryAll}</button>
            <button onClick={() => handleExpiry('expired')} className={pill(expiry === 'expired')}>{f.expiryExpired}</button>
            <button onClick={() => handleExpiry('active')} className={pill(expiry === 'active')}>{f.expiryActive}</button>
          </div>
        </div>
      </div>

      {/* Table */}
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
                  <th className={th}>{s.columns.message}</th>
                  <th className={`${thSort} hidden sm:table-cell`} onClick={() => handleSort('answer')}>
                    <div className="flex items-center gap-1">
                      {s.columns.answer}
                      <SortIcon field="answer" sort={sort} dir={dir} />
                    </div>
                  </th>
                  <th className={`${thSort} hidden md:table-cell`} onClick={() => handleSort('expiresAt')}>
                    <div className="flex items-center gap-1">
                      {s.columns.expiresAt}
                      <SortIcon field="expiresAt" sort={sort} dir={dir} />
                    </div>
                  </th>
                  <th className={`${thSort} hidden lg:table-cell`} onClick={() => handleSort('createdAt')}>
                    <div className="flex items-center gap-1">
                      {s.columns.createdAt}
                      <SortIcon field="createdAt" sort={sort} dir={dir} />
                    </div>
                  </th>
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
            pageSize={pageSize}
            totalPages={data.totalPages}
            totalCount={data.totalCount}
            onPageChange={handlePage}
            onPageSizeChange={handlePageSize}
          />
        </>
      )}
    </div>
  )
}
