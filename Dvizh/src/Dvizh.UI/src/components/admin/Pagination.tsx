'use client'

import { ChevronLeft, ChevronRight } from 'lucide-react'
import { useAdminStrings } from './AdminLanguageContext'
import { PAGE_SIZE_OPTIONS } from '@/lib/constants'

interface PaginationProps {
  page: number
  pageSize: number
  totalPages: number
  totalCount: number
  onPageChange: (page: number) => void
  onPageSizeChange: (size: number) => void
}

function getPageNumbers(current: number, total: number): (number | '...')[] {
  if (total <= 7) return Array.from({ length: total }, (_, i) => i + 1)

  const result: (number | '...')[] = [1]
  if (current > 3) result.push('...')
  for (let i = Math.max(2, current - 1); i <= Math.min(total - 1, current + 1); i++) {
    result.push(i)
  }
  if (current < total - 2) result.push('...')
  result.push(total)
  return result
}

export function Pagination({ page, pageSize, totalPages, totalCount, onPageChange, onPageSizeChange }: PaginationProps) {
  const { strings } = useAdminStrings()
  const f = strings.admin.filters

  const from = Math.min((page - 1) * pageSize + 1, totalCount)
  const to = Math.min(page * pageSize, totalCount)
  const pages = getPageNumbers(page, totalPages)

  const btn = 'w-8 h-8 flex items-center justify-center rounded-lg text-sm transition-colors'

  return (
    <div className="flex flex-wrap items-center justify-between gap-3 pt-4">
      <span className="text-xs text-gray-400">{f.showing(from, to, totalCount)}</span>

      <div className="flex items-center gap-3">
        <div className="flex items-center gap-1">
          {PAGE_SIZE_OPTIONS.map(size => (
            <button
              key={size}
              onClick={() => onPageSizeChange(size)}
              className={`px-2.5 py-1 rounded-lg text-xs font-medium transition-colors border ${
                pageSize === size
                  ? 'bg-emerald-50 border-emerald-300 text-emerald-700'
                  : 'bg-white border-gray-200 text-gray-500 hover:border-gray-300'
              }`}
            >
              {size}
            </button>
          ))}
          <span className="text-xs text-gray-400 ml-0.5">{f.perPage}</span>
        </div>

        {totalPages > 1 && (
          <div className="flex items-center gap-0.5">
            <button
              onClick={() => onPageChange(page - 1)}
              disabled={page <= 1}
              className={`${btn} text-gray-500 hover:bg-gray-100 disabled:opacity-30 disabled:cursor-not-allowed`}
            >
              <ChevronLeft className="w-4 h-4" />
            </button>

            {pages.map((p, i) =>
              p === '...'
                ? <span key={`ellipsis-${i}`} className="w-8 h-8 flex items-center justify-center text-sm text-gray-300">…</span>
                : <button
                    key={p}
                    onClick={() => onPageChange(p)}
                    className={`${btn} ${
                      p === page
                        ? 'bg-emerald-500 text-white font-medium'
                        : 'text-gray-500 hover:bg-gray-100'
                    }`}
                  >
                    {p}
                  </button>
            )}

            <button
              onClick={() => onPageChange(page + 1)}
              disabled={page >= totalPages}
              className={`${btn} text-gray-500 hover:bg-gray-100 disabled:opacity-30 disabled:cursor-not-allowed`}
            >
              <ChevronRight className="w-4 h-4" />
            </button>
          </div>
        )}
      </div>
    </div>
  )
}
