import { Suspense } from 'react'
import { UniversesTable } from '@/components/universes/UniversesTable'
import { Spinner } from '@/components/ui/Spinner'

export const metadata = { title: 'Universes — Lore' }

export default function UniversesPage() {
  return (
    <main className="max-w-5xl mx-auto px-4 py-8">
      <Suspense fallback={<div className="flex justify-center py-12"><Spinner /></div>}>
        <UniversesTable />
      </Suspense>
    </main>
  )
}
