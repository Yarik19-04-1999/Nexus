'use client'

import { RewatchStatus, REWATCH_STATUS_LABELS } from '@/types/movie'

interface Filters {
  rewatchStatuses: RewatchStatus[]
  scoreMin: string
  scoreMax: string
  yearMin: string
  yearMax: string
}

interface MovieFiltersProps {
  filters: Filters
  onChange: (filters: Filters) => void
}

const ALL_STATUSES: RewatchStatus[] = [0, 1, 2]

export function MovieFilters({ filters, onChange }: MovieFiltersProps) {
  const toggleStatus = (s: RewatchStatus) => {
    const next = filters.rewatchStatuses.includes(s)
      ? filters.rewatchStatuses.filter(x => x !== s)
      : [...filters.rewatchStatuses, s]
    onChange({ ...filters, rewatchStatuses: next })
  }

  return (
    <aside className="w-64 shrink-0 flex flex-col gap-6">
      <div>
        <h3 className="text-xs font-semibold text-gray-500 uppercase tracking-wide mb-3">Rewatch status</h3>
        <div className="flex flex-col gap-2">
          {ALL_STATUSES.map(s => (
            <label key={s} className="flex items-center gap-2.5 cursor-pointer">
              <input
                type="checkbox"
                checked={filters.rewatchStatuses.includes(s)}
                onChange={() => toggleStatus(s)}
                className="w-4 h-4 rounded border-gray-300 text-indigo-600 focus:ring-indigo-300"
              />
              <span className="text-sm text-gray-700">{REWATCH_STATUS_LABELS[s]}</span>
            </label>
          ))}
        </div>
      </div>

      <div>
        <h3 className="text-xs font-semibold text-gray-500 uppercase tracking-wide mb-3">Score</h3>
        <div className="flex items-center gap-2">
          <input
            type="number"
            placeholder="Min"
            value={filters.scoreMin}
            onChange={e => onChange({ ...filters, scoreMin: e.target.value })}
            min={0} max={10} step={0.1}
            className="w-full rounded-lg border border-gray-200 px-2.5 py-1.5 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-300"
          />
          <span className="text-gray-400 text-sm">–</span>
          <input
            type="number"
            placeholder="Max"
            value={filters.scoreMax}
            onChange={e => onChange({ ...filters, scoreMax: e.target.value })}
            min={0} max={10} step={0.1}
            className="w-full rounded-lg border border-gray-200 px-2.5 py-1.5 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-300"
          />
        </div>
      </div>

      <div>
        <h3 className="text-xs font-semibold text-gray-500 uppercase tracking-wide mb-3">Release year</h3>
        <div className="flex items-center gap-2">
          <input
            type="number"
            placeholder="From"
            value={filters.yearMin}
            onChange={e => onChange({ ...filters, yearMin: e.target.value })}
            className="w-full rounded-lg border border-gray-200 px-2.5 py-1.5 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-300"
          />
          <span className="text-gray-400 text-sm">–</span>
          <input
            type="number"
            placeholder="To"
            value={filters.yearMax}
            onChange={e => onChange({ ...filters, yearMax: e.target.value })}
            className="w-full rounded-lg border border-gray-200 px-2.5 py-1.5 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-300"
          />
        </div>
      </div>

      <button
        onClick={() => onChange({ rewatchStatuses: [], scoreMin: '', scoreMax: '', yearMin: '', yearMax: '' })}
        className="text-xs text-gray-400 hover:text-gray-600 text-left transition-colors"
      >
        Clear filters
      </button>
    </aside>
  )
}
