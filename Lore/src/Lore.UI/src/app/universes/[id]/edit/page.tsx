'use client'

import { use } from 'react'
import { useRouter } from 'next/navigation'
import { ArrowLeft } from 'lucide-react'
import { UniverseForm } from '@/components/universes/UniverseForm'
import { useUniverseById, useUpdateUniverse } from '@/hooks/useUniverses'
import { Spinner } from '@/components/ui/Spinner'
import type { CreateUniversePayload, UpdateUniversePayload } from '@/lib/api/universes'

interface Props {
  params: Promise<{ id: string }>
}

export default function EditUniversePage({ params }: Props) {
  const { id } = use(params)
  const universeId = Number(id)
  const router = useRouter()
  const { data: universe, isPending, isError } = useUniverseById(universeId)
  const updateUniverse = useUpdateUniverse()

  const handleSubmit = async (payload: CreateUniversePayload | UpdateUniversePayload) => {
    await updateUniverse.mutateAsync(payload as UpdateUniversePayload)
    router.push('/universes')
  }

  return (
    <main className="max-w-2xl mx-auto px-4 py-8">
      <button
        onClick={() => router.push('/universes')}
        className="flex items-center gap-1.5 text-sm text-gray-500 hover:text-gray-700 mb-6 transition-colors"
      >
        <ArrowLeft className="w-4 h-4" />
        Back to universes
      </button>

      <div className="bg-white rounded-2xl border border-gray-100 p-6 shadow-sm">
        <h1 className="text-lg font-bold text-gray-800 mb-6">Edit universe</h1>

        {isPending && (
          <div className="flex justify-center py-8"><Spinner /></div>
        )}

        {isError && (
          <p className="text-center text-rose-500 py-8">Universe not found.</p>
        )}

        {universe && (
          <UniverseForm
            universe={universe}
            onSubmit={handleSubmit}
            isPending={updateUniverse.isPending}
          />
        )}
      </div>
    </main>
  )
}
