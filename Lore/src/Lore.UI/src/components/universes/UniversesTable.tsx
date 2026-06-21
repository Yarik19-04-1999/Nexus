'use client'

import { useState, useEffect, useCallback } from 'react'
import Link from 'next/link'
import { useRouter, useSearchParams, usePathname } from 'next/navigation'
import { Plus, Search, ChevronUp, ChevronDown, ChevronsUpDown, EyeOff } from 'lucide-react'
import { useUniversesList, useCreateUniverse, useUpdateUniverse } from '@/hooks/useUniverses'
import { UniverseActions } from './UniverseActions'
import { UniverseModal } from './UniverseModal'
import { Pagination } from './Pagination'
import { Spinner } from '@/components/ui/Spinner'
import { DEFAULT_PAGE_SIZE } from '@/lib/constants'
import type { UniverseListParams, CreateUniversePayload, UpdateUniversePayload } from '@/lib/api/universes'
import type { Universe } from '@/types/universe'

type SortDir = 'asc' | 'desc'

function SortIcon({ field, sort, dir }: { field: string; sort: string | null; dir: SortDir }) {
  if (sort !== field) return <ChevronsUpDown className="w-3 h-3 text-gray-300" />
  return dir === 'asc'
    ? <ChevronUp className="w-3 h-3 text-indigo-500" />
    : <ChevronDown className="w-3 h-3 text-indigo-500" />
}

export function UniversesTable() {
  const router = useRouter()
  const pathname = usePathname()
  const searchParams = useSearchParams()
  const [createOpen, setCreateOpen] = useState(false)
  const [editTarget, setEditTarget] = useState<Universe | null>(null)
  const createUniverse = useCreateUniverse()
  const updateUniverse = useUpdateUniverse()

  const urlQ = searchParams.get('q') ?? ''
  const sort = searchParams.get('sort')
  const dir = (searchParams.get('dir') ?? 'asc') as SortDir
  const page = Number(searchParams.get('page') ?? '1')
  const pageSize = Number(searchParams.get('pageSize') ?? String(DEFAULT_PAGE_SIZE))

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

  useEffect(() => {
    const timer = setTimeout(() => {
      if (localQ !== urlQ) updateParams({ q: localQ || null, page: '1' })
    }, 300)
    return () => clearTimeout(timer)
  }, [localQ]) // eslint-disable-line react-hooks/exhaustive-deps

  function handleSort(field: string) {
    if (sort === field) {
      updateParams({ sort: field, dir: dir === 'asc' ? 'desc' : 'asc' })
    } else {
      updateParams({ sort: field, dir: 'asc' })
    }
  }

  const listParams: UniverseListParams = { page, pageSize, q: urlQ || undefined, sort: sort ?? undefined, dir }
  const { data, isPending } = useUniversesList(listParams)

  const handleCreate = async (payload: CreateUniversePayload | UpdateUniversePayload) => {
    await createUniverse.mutateAsync(payload as CreateUniversePayload)
    setCreateOpen(false)
  }

  const handleUpdate = async (payload: CreateUniversePayload | UpdateUniversePayload) => {
    await updateUniverse.mutateAsync(payload as UpdateUniversePayload)
    setEditTarget(null)
  }

  const thSort = 'px-4 py-3 font-medium cursor-pointer select-none'
  const th = 'px-4 py-3 font-medium'

  return (
    <div className="flex flex-col gap-4">
      <div className="flex items-center justify-between gap-3">
        <h1 className="text-xl font-bold text-gray-800">Universes</h1>
        <button
          onClick={() => setCreateOpen(true)}
          className="flex items-center gap-2 px-4 py-2 rounded-xl text-sm font-medium text-white bg-indigo-500 hover:bg-indigo-600 transition-colors"
        >
          <Plus className="w-4 h-4" />
          New universe
        </button>
      </div>

      <div className="relative">
        <Search className="absolute left-3 top-1/2 -translate-y-1/2 w-3.5 h-3.5 text-gray-400" />
        <input
          type="text"
          value={localQ}
          onChange={e => setLocalQ(e.target.value)}
          placeholder="Search by name…"
          className="w-full pl-9 pr-3 py-2 rounded-xl border border-gray-200 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-300"
        />
      </div>

      {isPending ? (
        <div className="flex justify-center py-12"><Spinner /></div>
      ) : !data?.items.length ? (
        <p className="text-center text-gray-400 py-12">No universes yet</p>
      ) : (
        <>
          <div className="overflow-x-auto rounded-2xl border border-gray-100">
            <table className="w-full text-sm">
              <thead>
                <tr className="bg-gray-50 text-left text-gray-500 text-xs uppercase tracking-wide">
                  <th className={`${thSort}`} onClick={() => handleSort('name')}>
                    <div className="flex items-center gap-1">
                      Name
                      <SortIcon field="name" sort={sort} dir={dir} />
                    </div>
                  </th>
                  <th className={`${th} hidden md:table-cell`}>Description</th>
                  <th className={`${thSort} hidden sm:table-cell`} onClick={() => handleSort('listNo')}>
                    <div className="flex items-center gap-1">
                      Order
                      <SortIcon field="listNo" sort={sort} dir={dir} />
                    </div>
                  </th>
                  <th className="px-4 py-3" />
                </tr>
              </thead>
              <tbody className="divide-y divide-gray-50">
                {data.items.map(universe => (
                  <tr key={universe.id} className="hover:bg-gray-50/50 transition-colors">
                    <td className="px-4 py-3">
                      <div className="flex items-center gap-2">
                        <Link href={`/universes/${universe.id}`} className="font-medium text-gray-800 hover:text-indigo-600 transition-colors">
                          {universe.name}
                        </Link>
                        {universe.isHidden && (
                          <span title="Hidden"><EyeOff className="w-3.5 h-3.5 text-gray-400" /></span>
                        )}
                      </div>
                    </td>
                    <td className="px-4 py-3 text-gray-500 hidden md:table-cell">
                      <span className="line-clamp-1">{universe.description ?? '—'}</span>
                    </td>
                    <td className="px-4 py-3 text-gray-500 hidden sm:table-cell">{universe.listNo}</td>
                    <td className="px-4 py-3">
                      <UniverseActions universe={universe} onEdit={setEditTarget} />
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
            onPageChange={p => updateParams({ page: String(p) })}
            onPageSizeChange={size => updateParams({ pageSize: String(size), page: '1' })}
          />
        </>
      )}

      <UniverseModal
        open={createOpen}
        isPending={createUniverse.isPending}
        onClose={() => setCreateOpen(false)}
        onSubmit={handleCreate}
      />
      <UniverseModal
        open={!!editTarget}
        universe={editTarget ?? undefined}
        isPending={updateUniverse.isPending}
        onClose={() => setEditTarget(null)}
        onSubmit={handleUpdate}
      />
    </div>
  )
}
