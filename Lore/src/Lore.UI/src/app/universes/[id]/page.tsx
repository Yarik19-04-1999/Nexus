import { Suspense } from 'react'
import { UniverseDetailPage } from '@/components/universes/UniverseDetailPage'
import { Spinner } from '@/components/ui/Spinner'

interface Props {
  params: Promise<{ id: string }>
}

export async function generateMetadata({ params }: Props) {
  const { id } = await params
  return { title: `Universe #${id} — Lore` }
}

export default async function Page({ params }: Props) {
  const { id } = await params
  return (
    <Suspense fallback={<div className="flex justify-center py-16"><Spinner /></div>}>
      <UniverseDetailPage universeId={Number(id)} />
    </Suspense>
  )
}
