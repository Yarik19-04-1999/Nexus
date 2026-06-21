import { Suspense } from 'react'
import { MoviesPage } from '@/components/movies/MoviesPage'
import { Spinner } from '@/components/ui/Spinner'

export const metadata = { title: 'Movies — Lore' }

export default function Page() {
  return (
    <Suspense fallback={<div className="flex justify-center py-12"><Spinner /></div>}>
      <MoviesPage />
    </Suspense>
  )
}
